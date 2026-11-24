using System.Net.Http.Json;
using System.Text.Json;
using NextHire_API.Domains;
using NextHire_API.DTOs.AvaliacaoAgenteDTO;
using NextHire_API.DTOs.QuestaoDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Applications.Services;

/// <summary>
/// Cliente do Next Agent: consome diretamente o LM Studio (API compatível OpenAI)
/// para gerar questões da vaga. NÃO usar gateway do OpenClaw como intermediário
/// (harness de tools induz o modelo de 7B a invocar a operação como ferramenta).
/// Os system prompts vêm dos arquivos versionados em Config/NextAgent/ (pacote
/// de integração do agente), com fallback embutido. A requisição usa response_format
/// json_schema (grammar estruturada): garante JSON parseável e a quantidade exata
/// de questões; a validação abaixo cobre alternativas, gabarito e duplicatas,
/// repetindo a chamada até 3 vezes (modelo estocástico).
/// </summary>
public class NextAgentService
{
    private readonly HttpClient _http;

    private const string Modelo = "qwen2.5-7b-instruct";
    private const int MaxTentativas = 3;

    /// <summary>Pasta (relativa à saída do build) com os arquivos de config do agente.</summary>
    private const string PastaConfig = "Config/NextAgent";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private const string DificuldadesValidas = "facil|media|dificil";

    // ======================================================================
    // System prompts versionados do pacote de integração do Next Agent
    // ======================================================================

    private const string PromptGerarQuestao =
        """
        You are the question-generation engine of Next Hire (a job-application platform).
        You receive a job description (vaga) as JSON and generate questions for candidates.

        OUTPUT RULES (absolute):
        - Reply with ONE JSON object and nothing else. No prose, no markdown, no code fences.
        - The first characters of your reply must be {"questoes":
        - All question text and alternatives in the language of the job ("language" field).
        - enums and JSON field names stay in technical form (not translated).

        Generate EXACTLY 10 questions, one per slot below, in this exact order:

        | Slot | classificacao_questao | tipo_questao | dificuldade | nº alternativas |
        |------|----------------------|--------------|-------------|-----------------|
        | Q1 | comportamental | verdadeiro_falso | media | 2 |
        | Q2 | comportamental | unica_escolha | media | 5 |
        | Q3 | comportamental | unica_escolha | dificil | 5 |
        | Q4 | tecnica | unica_escolha | facil | 5 |
        | Q5 | tecnica | multipla_escolha | facil | 5 |
        | Q6 | tecnica | dissertativa | dificil | 0 |
        | Q7 | tecnica | unica_escolha | media | 5 |
        | Q8 | situacional | multipla_escolha | dificil | 5 |
        | Q9 | situacional | dissertativa | facil | 0 |
        | Q10 | situacional | dissertativa | media | 0 |

        The "nº alternativas" column is the exact number of objects in "alternativas"
        for that question. Finish writing ALL alternatives of a question before
        starting the next one. After each objective question, count: 1, 2, 3, 4, 5.

        Output schema:
        {"questoes":[{"texto_questao":"...","classificacao_questao":"...","tipo_questao":"...","dificuldade":"...","alternativas":[{"texto_alternativa":"...","alternativa_correta":true}]}]}

        Hard rules:
        - unica_escolha and multipla_escolha: EXACTLY 5 alternatives — count them (5 objects in "alternativas"). unica_escolha: exactly 1 alternativa_correta=true. multipla_escolha: 1 to 4 true.
        - verdadeiro_falso: exactly 2 alternatives: "Verdadeiro" and "Falso", exactly 1 true.
        - dissertativa: "alternativas": [] (empty array).
        - Difficulty must fit the job experience_level.
        - Technical questions: unambiguous correct answer; distractors plausible but clearly wrong.
        - Situational questions: realistic work scenarios tied to the job's responsibilities.
        - Behavioral questions: mapped to the job's behavioral_skills.
        - No ids, no vaga_id, no extra fields.
        """;

    private const string PromptGerarMaisQuestoes =
        """
        You are the extra-question-generation engine of Next Hire.
        You receive a JSON with the job (vaga) and the questions already asked
        (questoes_existentes) to avoid duplicates.

        OUTPUT RULES (absolute):
        - Reply with ONE JSON object and nothing else, same schema as {"questoes":[...]}.
        - The first characters of your reply must be {"questoes": . No prose, no markdown.
        - Generate EXACTLY 5 NEW questions. At least 1 dissertativa; the others unica_escolha (exactly 5 alternatives, 1 correct) or multipla_escolha (5 alternatives, 1-4 correct) or verdadeiro_falso (2 alternatives).
        - Compare with questoes_existentes and do NOT repeat scenario, question text or competency.
        - All text in the language of the job ("language" field). Difficulty must fit experience_level.
        - No ids, no extra fields.
        """;

    private const string PromptAvaliacaoUsuario =
        """
        You are the candidate-evaluation engine of Next Hire (a job-application platform).
        You receive a JSON with the job (vaga), the candidate's answers (with status
        acertou/errou for objective questions) and the candidate's resume (curriculo).

        OUTPUT RULES (absolute):
        - Reply with ONE JSON object and nothing else: {"nota_geral":0,"comentario_respostas":"...","comentario_curriculo":"..."}
        - The first character of your reply must be "{" and the last "}". No prose, no markdown, no code fences.
        - All text in the language of the job ("language" field).
        - nota_geral: integer 0-100, weighting: objective-question status (from the backend) + quality of the dissertative answers + coherence with the resume.
        - comentario_respostas: direct human tone, citing real evidence (e.g., "na dissertativa 4 ele descreveu..."), distinguishing missing evidence from negative evidence, flagging inconsistencies.
        - comentario_curriculo: job x resume comparison (matches, gaps, points to verify in interview). Use only received data - never invent experiences, technologies or facts.
        - No extra fields, no hiring decision, no ranking.
        - If resume or answers are missing, narrow the scope and explain it in the comments.
        - If the respostas array is empty, state explicitly in comentario_respostas that the candidate has not answered any question of this job yet; never mention any answer, question number or dissertative.
        - Cite only items present in the received data. Never invent question numbers, answers, experiences, technologies or facts.
        """;

    public NextAgentService(HttpClient http)
        => _http = http;

    /// <summary>Operação generate_question: 10 questões (10 por slot fixo).</summary>
    public async Task<List<QuestaoAgenteDto>> GerarQuestoes(object payloadVaga)
    {
        var payload = JsonSerializer.Serialize(new
        {
            operacao = "generate_question",
            quantidade = 10,
            vaga = payloadVaga
        });

        return await ChamarAgenteAsync(
            CarregarPromptConfig("system-prompt-generate-question.md", PromptGerarQuestao),
            payload, quantidadeEsperada: 10);
    }

    /// <summary>Operação generate_more_question: 5 questões novas sem repetir as existentes.</summary>
    public async Task<List<QuestaoAgenteDto>> GerarMaisQuestoes(object payloadVaga, List<object> questoesExistentes)
    {
        var payload = JsonSerializer.Serialize(new
        {
            operacao = "generate_more_question",
            quantidade = 5,
            vaga = payloadVaga,
            questoes_existentes = questoesExistentes
        });

        return await ChamarAgenteAsync(
            CarregarPromptConfig("system-prompt-generate-more-questions.md", PromptGerarMaisQuestoes),
            payload, quantidadeEsperada: 5);
    }

    /// <summary>
    /// Operação evaluation_user: avalia a candidatura de um candidato a uma vaga
    /// (respostas + currículo) e retorna nota geral e comentários do agente.
    /// </summary>
    public async Task<AvaliacaoAgenteDto> AvaliarCandidatura(object payloadCandidatura)
    {
        var payload = JsonSerializer.Serialize(new
        {
            operacao = "evaluation_user",
            candidatura = payloadCandidatura
        });

        return await ChamarComValidacaoAsync(
            CarregarPromptConfig("system-prompt-evaluation-user.md", PromptAvaliacaoUsuario),
            payload,
            CriarResponseFormatAvaliacao(),
            conteudo =>
            {
                var avaliacao = ExtrairAvaliacao(conteudo);
                ValidarAvaliacao(avaliacao);
                return avaliacao;
            },
            descricaoRetorno: "avaliação válida");
    }

    // ======================================================================
    // Chamada HTTP + validação obrigatória com retry
    // ======================================================================

    /// <summary>
    /// Carrega o system prompt da config versionada do Next Agent
    /// (arquivos .md do pacote de integração, copiados para a saída do build).
    /// Se o arquivo não existir, usa o prompt embutido como fallback.
    /// </summary>
    private static string CarregarPromptConfig(string nomeArquivo, string fallback)
    {
        try
        {
            var caminho = Path.Combine(AppContext.BaseDirectory, PastaConfig, nomeArquivo);

            if (File.Exists(caminho))
            {
                var conteudo = File.ReadAllText(caminho).Trim();
                if (!string.IsNullOrWhiteSpace(conteudo))
                {
                    Console.WriteLine($"[NextAgent] Config carregada do arquivo: {PastaConfig}/{nomeArquivo}");
                    return conteudo;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NextAgent] Falha ao carregar config '{nomeArquivo}': {ex.Message}. Usando prompt embutido.");
        }

        Console.WriteLine($"[NextAgent] Usando prompt embutido (fallback): {nomeArquivo}");
        return fallback;
    }

    private async Task<List<QuestaoAgenteDto>> ChamarAgenteAsync(string systemPrompt, string userPayload, int quantidadeEsperada)
        => await ChamarComValidacaoAsync(
            systemPrompt,
            userPayload,
            CriarResponseFormatQuestoes(quantidadeEsperada),
            conteudo =>
            {
                var questoes = ExtrairQuestoes(conteudo);
                ValidarQuestoes(questoes, quantidadeEsperada);
                return questoes;
            },
            descricaoRetorno: "questões válidas");

    /// <summary>
    /// Grammar estruturada das operações de geração de questões:
    /// garante JSON parseável e a quantidade exata de questões (minItems/maxItems).
    /// A validação em código cobre o restante (alternativas, gabarito).
    /// </summary>
    private static object CriarResponseFormatQuestoes(int quantidadeEsperada)
        => new
        {
            type = "json_schema",
            json_schema = new
            {
                name = "questoes_vaga",
                strict = true,
                schema = new
                {
                    type = "object",
                    properties = new Dictionary<string, object>
                    {
                        ["questoes"] = new Dictionary<string, object>
                        {
                            ["type"] = "array",
                            ["items"] = new Dictionary<string, object>
                            {
                                ["type"] = "object",
                                ["properties"] = new Dictionary<string, object>
                                {
                                    ["texto_questao"] = new Dictionary<string, object> { ["type"] = "string" },
                                    ["classificacao_questao"] = new Dictionary<string, object> { ["type"] = "string" },
                                    ["tipo_questao"] = new Dictionary<string, object> { ["type"] = "string" },
                                    ["dificuldade"] = new Dictionary<string, object> { ["type"] = "string" },
                                    ["alternativas"] = new Dictionary<string, object>
                                    {
                                        ["type"] = "array",
                                        ["items"] = new Dictionary<string, object>
                                        {
                                            ["type"] = "object",
                                            ["properties"] = new Dictionary<string, object>
                                            {
                                                ["texto_alternativa"] = new Dictionary<string, object> { ["type"] = "string" },
                                                ["alternativa_correta"] = new Dictionary<string, object> { ["type"] = "boolean" }
                                            }
                                        }
                                    }
                                }
                            },
                            ["minItems"] = quantidadeEsperada,
                            ["maxItems"] = quantidadeEsperada
                        }
                    },
                    required = new[] { "questoes" }
                }
            }
        };

    /// <summary>Grammar estruturada da operação evaluation_user.</summary>
    private static object CriarResponseFormatAvaliacao()
        => new
        {
            type = "json_schema",
            json_schema = new
            {
                name = "avaliacao_candidatura",
                strict = true,
                schema = new
                {
                    type = "object",
                    properties = new Dictionary<string, object>
                    {
                        ["nota_geral"] = new Dictionary<string, object>
                        {
                            ["type"] = "integer",
                            ["minimum"] = 0,
                            ["maximum"] = 100
                        },
                        ["comentario_respostas"] = new Dictionary<string, object> { ["type"] = "string" },
                        ["comentario_curriculo"] = new Dictionary<string, object> { ["type"] = "string" }
                    },
                    required = new[] { "nota_geral", "comentario_respostas", "comentario_curriculo" }
                }
            }
        };

    /// <summary>
    /// Executa a operação no LM Studio com a grammar estruturada informada,
    /// extrai o JSON da resposta e delega a interpretação/validação para quem chamou.
    /// Repete até MaxTentativas (modelo estocástico).
    /// </summary>
    private async Task<T> ChamarComValidacaoAsync<T>(
        string systemPrompt,
        string userPayload,
        object responseFormat,
        Func<string?, T> interpretar,
        string descricaoRetorno)
    {
        var corpo = new Dictionary<string, object?>
        {
            ["model"] = Modelo,
            ["messages"] = new object[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPayload }
            },
            ["temperature"] = 0.2,
            ["max_tokens"] = 8192,
            ["stream"] = false,
            ["response_format"] = responseFormat
        };

        Exception? ultimoErro = null;

        for (int tentativa = 1; tentativa <= MaxTentativas; tentativa++)
        {
            try
            {
                var resposta = await _http.PostAsJsonAsync("/v1/chat/completions", corpo);
                resposta.EnsureSuccessStatusCode();

                var completions = await resposta.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
                var conteudo = completions
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return interpretar(conteudo);
            }
            catch (Exception ex)
            {
                ultimoErro = ex;
                Console.WriteLine($"[NextAgent] Tentativa {tentativa}/{MaxTentativas} falhou: {ex.Message}");
                await Task.Delay(500 * tentativa);
            }
        }

        throw new DomainException($"O agente não retornou {descricaoRetorno} após {MaxTentativas} tentativas: {ultimoErro?.Message}");
    }

    /// <summary>Extrai o JSON da resposta (o modelo pode incluir prose ou code fences).</summary>
    private static List<QuestaoAgenteDto> ExtrairQuestoes(string? conteudo)
    {
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new Exception("Resposta vazia do agente.");

        var inicio = conteudo.IndexOf('{');
        var fim = conteudo.LastIndexOf('}');

        if (inicio < 0 || fim <= inicio)
            throw new Exception("Resposta do agente não contém JSON.");

        var resposta = JsonSerializer.Deserialize<AgenteQuestoesRespostaDto>(
            conteudo.Substring(inicio, fim - inicio + 1), JsonOptions);

        if (resposta?.questoes == null || resposta.questoes.Count == 0)
            throw new Exception("Resposta do agente sem questões.");

        return resposta.questoes;
    }

    /// <summary>Extrai o JSON da avaliação (o modelo pode incluir prose ou code fences).</summary>
    private static AvaliacaoAgenteDto ExtrairAvaliacao(string? conteudo)
    {
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new Exception("Resposta vazia do agente.");

        var inicio = conteudo.IndexOf('{');
        var fim = conteudo.LastIndexOf('}');

        if (inicio < 0 || fim <= inicio)
            throw new Exception("Resposta do agente não contém JSON.");

        var avaliacao = JsonSerializer.Deserialize<AvaliacaoAgenteDto>(
            conteudo.Substring(inicio, fim - inicio + 1), JsonOptions);

        if (avaliacao == null)
            throw new Exception("Resposta do agente sem avaliação.");

        return avaliacao;
    }

    /// <summary>
    /// Validação obrigatória da avaliação (modelo estocástico):
    /// nota_geral inteiro entre 0 e 100 e os dois comentários preenchidos.
    /// </summary>
    private static void ValidarAvaliacao(AvaliacaoAgenteDto avaliacao)
    {
        if (avaliacao.nota_geral is < 0 or > 100)
            throw new Exception($"nota_geral fora do intervalo 0-100: {avaliacao.nota_geral}.");

        if (string.IsNullOrWhiteSpace(avaliacao.comentario_respostas))
            throw new Exception("Avaliação sem comentário sobre as respostas.");

        if (string.IsNullOrWhiteSpace(avaliacao.comentario_curriculo))
            throw new Exception("Avaliação sem comentário sobre o currículo.");
    }

    /// <summary>
    /// Validação obrigatória (modelo estocástico):
    /// 1. quantidade exata de questões;
    /// 2. nº de alternativas por tipo (unica/multipla = 5, verdadeiro_falso = 2, dissertativa = 0);
    /// 3. classificacao_questao, tipo_questao e dificuldade dentro dos enums;
    /// 4. regras de gabarito (1 correta em unica/vf; 1 a 4 em multipla);
    /// 5. sem textos duplicados no lote.
    /// </summary>
    private static void ValidarQuestoes(List<QuestaoAgenteDto> questoes, int quantidadeEsperada)
    {
        if (questoes.Count != quantidadeEsperada)
            throw new Exception($"Agente retornou {questoes.Count} questões, esperado {quantidadeEsperada}.");

        var dificuldadesValidas = DificuldadesValidas.Split('|');

        foreach (var questao in questoes)
        {
            if (string.IsNullOrWhiteSpace(questao.texto_questao))
                throw new Exception("Questão sem texto retornada pelo agente.");

            var tipo = ConverterTipoQuestao(questao.tipo_questao);
            _ = ConverterClassificacaoQuestao(questao.classificacao_questao);

            if (!string.IsNullOrWhiteSpace(questao.dificuldade)
                && !dificuldadesValidas.Contains(questao.dificuldade.Trim().ToLowerInvariant()))
                throw new Exception($"Dificuldade inválida: {questao.dificuldade}.");

            var totalCorretas = questao.alternativas.Count(a => a.alternativa_correta == true);

            switch (tipo)
            {
                case TipoQuestaoEnum.UnicaEscolha:
                    if (questao.alternativas.Count != 5 || totalCorretas != 1)
                        throw new Exception("unica_escolha exige 5 alternativas com exatamente 1 correta.");
                    break;

                case TipoQuestaoEnum.MultiplaEscolha:
                    if (questao.alternativas.Count != 5 || totalCorretas < 1 || totalCorretas > 4)
                        throw new Exception("multipla_escolha exige 5 alternativas com 1 a 4 corretas.");
                    break;

                case TipoQuestaoEnum.VerdadeiroFalso:
                    if (questao.alternativas.Count != 2 || totalCorretas != 1)
                        throw new Exception("verdadeiro_falso exige 2 alternativas com exatamente 1 correta.");
                    break;

                case TipoQuestaoEnum.Dissertativa:
                    if (questao.alternativas.Count != 0)
                        throw new Exception("dissertativa não pode ter alternativas.");
                    break;
            }
        }

        if (questoes.GroupBy(q => q.texto_questao.Trim(), StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1))
            throw new Exception("Agente retornou questões duplicadas no lote.");
    }

    // ======================================================================
    // Conversão das strings técnicas do agente para os enums do PostgreSQL
    // (Enum.TryParse não resolve: os PgName usam underscore)
    // ======================================================================

    public static TipoQuestaoEnum ConverterTipoQuestao(string valor)
        => valor.Trim().ToLowerInvariant() switch
        {
            "verdadeiro_falso" => TipoQuestaoEnum.VerdadeiroFalso,
            "unica_escolha" => TipoQuestaoEnum.UnicaEscolha,
            "multipla_escolha" => TipoQuestaoEnum.MultiplaEscolha,
            "dissertativa" => TipoQuestaoEnum.Dissertativa,
            _ => throw new DomainException($"tipo_questao inválido: {valor}. Valores aceitos: verdadeiro_falso, unica_escolha, multipla_escolha, dissertativa.")
        };

    public static ClassificacaoQuestaoEnum ConverterClassificacaoQuestao(string valor)
        => valor.Trim().ToLowerInvariant() switch
        {
            "comportamental" => ClassificacaoQuestaoEnum.Comportamental,
            "tecnica" => ClassificacaoQuestaoEnum.Tecnica,
            "situacional" => ClassificacaoQuestaoEnum.Situacional,
            _ => throw new DomainException($"classificacao_questao inválida: {valor}. Valores aceitos: comportamental, tecnica, situacional.")
        };

    /// <summary>Converte o enum do banco para a string técnica do contrato do agente.</summary>
    public static string ConverterTipoQuestaoParaString(TipoQuestaoEnum tipo)
        => tipo switch
        {
            TipoQuestaoEnum.VerdadeiroFalso => "verdadeiro_falso",
            TipoQuestaoEnum.UnicaEscolha => "unica_escolha",
            TipoQuestaoEnum.MultiplaEscolha => "multipla_escolha",
            _ => "dissertativa"
        };
}

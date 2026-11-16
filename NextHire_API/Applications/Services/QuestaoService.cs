using System.Text.Json;
using NextHire_API.Applications.Convertions.DomainToDTO;
using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoQuestaoDTO;
using NextHire_API.DTOs.QuestaoDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services;

/// <summary>
/// CRUD de questões + integração com o Next Agent.
/// Fluxos do agente: generate_question (10 questões por vaga) e
/// generate_more_question (5 novas, sem repetir as existentes).
/// </summary>
public class QuestaoService
{
    private readonly IQuestaoRepository _repository;
    private readonly NextAgentService _agente;
    private readonly ICandidato_QuestaoRepository _candidatoQuestaoRepository;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public QuestaoService(
        IQuestaoRepository repository,
        NextAgentService agente,
        ICandidato_QuestaoRepository candidatoQuestaoRepository)
    {
        _repository = repository;
        _agente = agente;
        _candidatoQuestaoRepository = candidatoQuestaoRepository;
    }

    // ======================================================================
    // Next Agent — generate_question (10 questões)
    // ======================================================================

    public async Task<List<ListarQuestaoDto>> GerarQuestoesDaVaga(int vagaId)
    {
        if (vagaId <= 0)
            throw new DomainException("Informe um ID de vaga válido.");

        var vaga = await _repository.BuscarVagaParaAgente(vagaId);

        var geradas = await _agente.GerarQuestoes(MontarPayloadVaga(vaga));

        var questoes = geradas
            .Select(q => new Questao
            {
                VagaId = vagaId,
                TextoQuestao = q.texto_questao.Trim(),
                ClasssificacaoQuestao = NextAgentService.ConverterClassificacaoQuestao(q.classificacao_questao),
                TipoQuestao = NextAgentService.ConverterTipoQuestao(q.tipo_questao),
                Alternativa = q.alternativas
                    .Select(a => new Alternativa
                    {
                        TextoAlternativa = a.texto_alternativa,
                        AlternativaCorreta = a.alternativa_correta == true
                    })
                    .ToList()
            })
            .ToList();

        await _repository.AdicionarVarias(questoes);

        return questoes.Select(QuestaoToDTO.QuestaoParaListarDto).ToList();
    }

    // ======================================================================
    // Next Agent — generate_more_question (5 questões novas)
    // ======================================================================

    public async Task<List<ListarQuestaoDto>> GerarMaisQuestoesDaVaga(int vagaId)
    {
        if (vagaId <= 0)
            throw new DomainException("Informe um ID de vaga válido.");

        var vaga = await _repository.BuscarVagaParaAgente(vagaId);
        var existentes = await _repository.ListarQuestoesExistentesPorVaga(vagaId);

        var contextoExistentes = existentes
            .Select(q => (object)new
            {
                texto_questao = q.TextoQuestao,
                classificacao_questao = q.ClasssificacaoQuestao.ToString().ToLowerInvariant(),
                tipo_questao = ConverterTipoParaString(q.TipoQuestao)
            })
            .ToList();

        var geradas = await _agente.GerarMaisQuestoes(MontarPayloadVaga(vaga), contextoExistentes);

        // Modelo estocástico: descarta repetições das questões já existentes na vaga.
        var textosExistentes = existentes
            .Select(q => q.TextoQuestao.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var novas = geradas
            .Where(q => !textosExistentes.Contains(q.texto_questao.Trim()))
            .ToList();

        if (novas.Count == 0)
            throw new DomainException("O agente não gerou questões novas: todas as questões retornadas já existem na vaga.");

        var questoes = novas
            .Select(q => new Questao
            {
                VagaId = vagaId,
                TextoQuestao = q.texto_questao.Trim(),
                ClasssificacaoQuestao = NextAgentService.ConverterClassificacaoQuestao(q.classificacao_questao),
                TipoQuestao = NextAgentService.ConverterTipoQuestao(q.tipo_questao),
                Alternativa = q.alternativas
                    .Select(a => new Alternativa
                    {
                        TextoAlternativa = a.texto_alternativa,
                        AlternativaCorreta = a.alternativa_correta == true
                    })
                    .ToList()
            })
            .ToList();

        await _repository.AdicionarVarias(questoes);

        return questoes.Select(QuestaoToDTO.QuestaoParaListarDto).ToList();
    }

    // ======================================================================
    // Listagens
    // ======================================================================

    public async Task<List<ListarQuestaoDto>> ListarPorVaga(int vagaId)
    {
        if (vagaId <= 0)
            throw new DomainException("Informe um ID de vaga válido.");

        var questoes = await _repository.ListarPorVaga(vagaId);

        if (questoes == null || questoes.Count == 0)
            throw new DomainException("Nenhuma questão encontrada para a vaga informada.");

        return questoes.Select(QuestaoToDTO.QuestaoParaListarDto).ToList();
    }

    public async Task<ListarQuestaoDto> BuscarPorId(int questaoId)
    {
        if (questaoId <= 0)
            throw new DomainException("Informe um ID de questão válido.");

        return QuestaoToDTO.QuestaoParaListarDto(await _repository.BuscarPorId(questaoId));
    }

    // ======================================================================
    // Create / Update / Delete (manual)
    // ======================================================================

    public async Task<ListarQuestaoDto> Adicionar(int vagaId, AdicionarQuestaoDto dto)
    {
        if (vagaId <= 0)
            throw new DomainException("Informe um ID de vaga válido.");

        if (dto == null)
            throw new DomainException("Informe os dados da questão.");

        await _repository.BuscarVagaParaAgente(vagaId);

        var questao = MontarDomain(vagaId, dto.textoQuestao, dto.classificacaoQuestao, dto.tipoQuestao, dto.alternativas);

        await _repository.Adicionar(questao);

        return QuestaoToDTO.QuestaoParaListarDto(await _repository.BuscarPorId(questao.QuestaoId));
    }

    public async Task Atualizar(int questaoId, AtualizarQuestaoDto dto)
    {
        if (questaoId <= 0)
            throw new DomainException("Informe um ID de questão válido.");

        if (dto == null)
            throw new DomainException("Informe os dados para atualização.");

        var atual = await _repository.BuscarPorId(questaoId);

        var texto = string.IsNullOrWhiteSpace(dto.textoQuestao) ? atual.TextoQuestao : dto.textoQuestao.Trim();

        var classificacao = string.IsNullOrWhiteSpace(dto.classificacaoQuestao)
            ? atual.ClasssificacaoQuestao
            : NextAgentService.ConverterClassificacaoQuestao(dto.classificacaoQuestao);

        var tipo = string.IsNullOrWhiteSpace(dto.tipoQuestao)
            ? atual.TipoQuestao
            : NextAgentService.ConverterTipoQuestao(dto.tipoQuestao);

        List<Alternativa>? alternativas = null;

        if (dto.alternativas != null)
        {
            ValidarAlternativas(tipo, dto.alternativas);

            alternativas = dto.alternativas
                .Select(a => new Alternativa
                {
                    TextoAlternativa = a.textoAlternativa.Trim(),
                    AlternativaCorreta = a.alternativaCorreta
                })
                .ToList();
        }
        else
        {
            // Tipo/classificação podem ter mudado: valida o gabarito atual contra o novo tipo.
            ValidarAlternativas(tipo, atual.Alternativa
                .Select(a => new CriarAlternativaDto
                {
                    textoAlternativa = a.TextoAlternativa,
                    alternativaCorreta = a.AlternativaCorreta
                })
                .ToList());
        }

        await _repository.Atualizar(questaoId, new Questao
        {
            TextoQuestao = texto,
            ClasssificacaoQuestao = classificacao,
            TipoQuestao = tipo
        }, alternativas);
    }

    public async Task Remover(int questaoId)
    {
        if (questaoId <= 0)
            throw new DomainException("Informe um ID de questão válido.");

        await _repository.Remover(questaoId);
    }

    // ======================================================================
    // Verificação das respostas do candidato (gabarito)
    // ======================================================================

    /// <summary>
    /// Compara as respostas do candidato com o gabarito das questões da vaga.
    /// acertou: true/false para questões objetivas e null para dissertativas.
    /// </summary>
    public async Task<List<VerificarQuestaoDto>> VerificarRespostas(int candidatoId, int vagaId)
    {
        if (candidatoId <= 0)
            throw new DomainException("Informe um ID de candidato válido.");

        if (vagaId <= 0)
            throw new DomainException("Informe um ID de vaga válido.");

        var respostas = await _candidatoQuestaoRepository.ListarRespostasComGabarito(candidatoId, vagaId);

        if (respostas == null || respostas.Count == 0)
            throw new DomainException("Nenhuma resposta encontrada para o candidato nesta vaga.");

        return respostas
            .Where(r => r != null)
            .Select(r => VerificarUma(r!))
            .ToList();
    }

    private static VerificarQuestaoDto VerificarUma(CandidatoQuestao resposta)
    {
        var questao = resposta.Questao!;
        var payload = DeserializarResposta(resposta.RepostaQuestao);

        var corretas = questao.Alternativa
            .Where(a => a.AlternativaCorreta)
            .ToList();

        return new VerificarQuestaoDto
        {
            questaoId = questao.QuestaoId,
            textoQuestao = questao.TextoQuestao,
            tipoQuestao = questao.TipoQuestao.ToString(),
            acertou = VerificarAcerto(questao, payload),
            respostasCandidato = ExtrairRespostasCandidato(questao, payload),
            respostasCorretas = corretas
                .Select(a => a.TextoAlternativa)
                .ToList()
        };
    }

    private static RespostaQuestaoPayloadDTO? DeserializarResposta(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<RespostaQuestaoPayloadDTO>(json, JsonOptions);
    }

    private static bool? VerificarAcerto(Questao questao, RespostaQuestaoPayloadDTO? payload)
    {
        if (payload == null)
            return false;

        if (questao.TipoQuestao == TipoQuestaoEnum.Dissertativa)
            return null;

        var textosCorretos = questao.Alternativa
            .Where(a => a.AlternativaCorreta)
            .Select(a => a.TextoAlternativa.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        switch (questao.TipoQuestao)
        {
            case TipoQuestaoEnum.VerdadeiroFalso:
                // O agente gera as alternativas "Verdadeiro" / "Falso";
                // mantém compatibilidade com o formato antigo "Sim" / "Não" e com booleanos.
                if (payload.resposta?.ValueKind == JsonValueKind.True)
                    return textosCorretos.Contains("Verdadeiro") || textosCorretos.Contains("Sim");
                if (payload.resposta?.ValueKind == JsonValueKind.False)
                    return textosCorretos.Contains("Falso") || textosCorretos.Contains("Não");

                var textoVf = payload.resposta?.GetString()?.Trim();
                return !string.IsNullOrEmpty(textoVf) && textosCorretos.Contains(textoVf);

            case TipoQuestaoEnum.UnicaEscolha:
                var textoEscolhido = payload.resposta?.GetString()?.Trim();
                return !string.IsNullOrEmpty(textoEscolhido) && textosCorretos.Contains(textoEscolhido);

            case TipoQuestaoEnum.MultiplaEscolha:
                return payload.respostas != null &&
                       payload.respostas.Count == textosCorretos.Count &&
                       textosCorretos.SetEquals(payload.respostas.Select(r => r.Trim()));

            default:
                return null;
        }
    }

    private static List<string> ExtrairRespostasCandidato(Questao questao, RespostaQuestaoPayloadDTO? payload)
    {
        var respostas = new List<string>();

        if (payload?.resposta != null)
        {
            switch (payload.resposta.Value.ValueKind)
            {
                case JsonValueKind.String:
                    var texto = payload.resposta.Value.GetString();
                    if (!string.IsNullOrWhiteSpace(texto))
                        respostas.Add(texto);
                    break;

                case JsonValueKind.True:
                    respostas.Add("true");
                    break;

                case JsonValueKind.False:
                    respostas.Add("false");
                    break;

                case JsonValueKind.Number:
                    if (payload.resposta.Value.TryGetInt32(out var alternativaId))
                    {
                        var alternativa = questao.Alternativa
                            .FirstOrDefault(a => a.AlternativaId == alternativaId);
                        if (alternativa != null)
                            respostas.Add(alternativa.TextoAlternativa);
                    }
                    break;
            }
        }

        if (payload?.respostas != null)
            respostas.AddRange(payload.respostas);

        return respostas;
    }

    // ======================================================================
    // Helpers
    // ======================================================================

    /// <summary>Valida as alternativas conforme o tipo da questão (mesmas regras do agente).</summary>
    private static void ValidarAlternativas(TipoQuestaoEnum tipo, List<CriarAlternativaDto> alternativas)
    {
        var totalCorretas = alternativas.Count(a => a.alternativaCorreta);

        switch (tipo)
        {
            case TipoQuestaoEnum.UnicaEscolha:
                if (alternativas.Count != 5 || totalCorretas != 1)
                    throw new DomainException("unica_escolha exige 5 alternativas com exatamente 1 correta.");
                break;

            case TipoQuestaoEnum.MultiplaEscolha:
                if (alternativas.Count != 5 || totalCorretas < 1 || totalCorretas > 4)
                    throw new DomainException("multipla_escolha exige 5 alternativas com 1 a 4 corretas.");
                break;

            case TipoQuestaoEnum.VerdadeiroFalso:
                if (alternativas.Count != 2 || totalCorretas != 1)
                    throw new DomainException("verdadeiro_falso exige 2 alternativas com exatamente 1 correta.");
                break;

            case TipoQuestaoEnum.Dissertativa:
                if (alternativas.Count != 0)
                    throw new DomainException("dissertativa não pode ter alternativas.");
                break;
        }

        if (alternativas.Any(a => string.IsNullOrWhiteSpace(a.textoAlternativa)))
            throw new DomainException("Toda alternativa precisa de texto.");
    }

    private static Questao MontarDomain(
        int vagaId,
        string textoQuestao,
        string classificacaoQuestao,
        string tipoQuestao,
        List<CriarAlternativaDto> alternativas)
    {
        if (string.IsNullOrWhiteSpace(textoQuestao))
            throw new DomainException("O texto da questão é obrigatório.");

        var tipo = NextAgentService.ConverterTipoQuestao(tipoQuestao);
        var classificacao = NextAgentService.ConverterClassificacaoQuestao(classificacaoQuestao);

        ValidarAlternativas(tipo, alternativas);

        return new Questao
        {
            VagaId = vagaId,
            TextoQuestao = textoQuestao.Trim(),
            ClasssificacaoQuestao = classificacao,
            TipoQuestao = tipo,
            Alternativa = alternativas
                .Select(a => new Alternativa
                {
                    TextoAlternativa = a.textoAlternativa.Trim(),
                    AlternativaCorreta = a.alternativaCorreta
                })
                .ToList()
        };
    }

    /// <summary>
    /// Monta o payload da vaga para o agente. O schema atual não possui
    /// behavioral_skills nem responsibilities: o agente trabalha sem esses campos.
    /// </summary>
    private static object MontarPayloadVaga(Vaga vaga)
    {
        var idiomas = DeserializarIdiomas(vaga.IdiomasDesejados);

        return new
        {
            title = vaga.Titulo,
            description = vaga.Descricao,
            area = vaga.Setor?.AreaOcupacao?.Nome,
            experience_level = DerivarNivelExperiencia(vaga.TempoExperienciaDesejado),
            occupation = vaga.Setor?.Nome,
            technical_skills = vaga.Habilidade?.Select(h => h.Nome).ToList() ?? new List<string>(),
            language = idiomas.FirstOrDefault() ?? "pt-BR"
        };
    }

    /// <summary>Deriva o nível de experiência a partir do tempo desejado (em anos).</summary>
    private static string DerivarNivelExperiencia(decimal? tempoExperiencia)
    {
        if (tempoExperiencia is null or <= 1)
            return "Júnior";

        if (tempoExperiencia <= 3)
            return "Pleno";

        return "Sênior";
    }

    private static List<string> DeserializarIdiomas(string? idiomasJson)
    {
        if (string.IsNullOrWhiteSpace(idiomasJson))
            return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(idiomasJson, JsonOptions) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    private static string ConverterTipoParaString(TipoQuestaoEnum tipo)
        => tipo switch
        {
            TipoQuestaoEnum.VerdadeiroFalso => "verdadeiro_falso",
            TipoQuestaoEnum.UnicaEscolha => "unica_escolha",
            TipoQuestaoEnum.MultiplaEscolha => "multipla_escolha",
            _ => "dissertativa"
        };
}

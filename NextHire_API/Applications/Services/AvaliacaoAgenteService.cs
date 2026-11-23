using System.Text.Json;
using NextHire_API.Domains;
using NextHire_API.DTOs.AvaliacaoAgenteDTO;
using NextHire_API.DTOs.QuestaoDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services;

/// <summary>
/// Avaliação de candidaturas pelo Next Agent (operação evaluation_user):
/// monta o payload a partir da vaga, das respostas do candidato e do currículo,
/// valida o retorno do agente e persiste nota + comentários no banco.
/// </summary>
public class AvaliacaoAgenteService
{
    private readonly NextAgentService _agente;
    private readonly QuestaoService _questaoService;
    private readonly IQuestaoRepository _questaoRepository;
    private readonly ICurriculoVagaRepository _curriculoVagaRepository;
    private readonly IAvaliacaoAgenteRepository _repository;

    public AvaliacaoAgenteService(
        NextAgentService agente,
        QuestaoService questaoService,
        IQuestaoRepository questaoRepository,
        ICurriculoVagaRepository curriculoVagaRepository,
        IAvaliacaoAgenteRepository repository)
    {
        _agente = agente;
        _questaoService = questaoService;
        _questaoRepository = questaoRepository;
        _curriculoVagaRepository = curriculoVagaRepository;
        _repository = repository;
    }

    /// <summary>
    /// Gera a avaliação do agente para a candidatura (candidato x vaga) e persiste
    /// nota geral + comentários sobre as respostas e sobre o currículo.
    /// </summary>
    public async Task<ListarAvaliacaoAgenteDto> AvaliarCandidatura(int vagaId, int candidatoId)
    {
        if (vagaId <= 0)
            throw new DomainException("Informe um ID de vaga válido.");

        if (candidatoId <= 0)
            throw new DomainException("Informe um ID de candidato válido.");

        var vaga = await _questaoRepository.BuscarVagaParaAgente(vagaId);

        var candidaturas = await _curriculoVagaRepository.ListarCandidatosPorVagaId(vagaId);
        var candidatura = candidaturas.FirstOrDefault(c => c.Curriculo.CandidatoId == candidatoId)
            ?? throw new DomainException("O candidato informado não possui candidatura para esta vaga.");

        var respostas = await _questaoService.VerificarRespostas(candidatoId, vagaId);

        var payload = new
        {
            vaga = new
            {
                title = vaga.Titulo,
                language = DeserializarIdiomas(vaga.IdiomasDesejados).FirstOrDefault() ?? "pt-BR",
                experience_level = DerivarNivelExperiencia(vaga.TempoExperienciaDesejado),
                technical_skills = vaga.Habilidade?.Select(h => h.Nome).ToList() ?? new List<string>()
            },
            candidato = new
            {
                nome = candidatura.Curriculo.Candidato.Nome,
                email = candidatura.Curriculo.Candidato.Email
            },
            respostas = respostas.Select(MontarRespostaPayload).ToList(),
            curriculo = NormalizarCurriculo(candidatura.Curriculo.CurriculoConvertido)
        };

        var avaliacao = await _agente.AvaliarCandidatura(payload);

        var salva = await _repository.Salvar(new AvaliacaoAgente
        {
            VagaId = vagaId,
            CandidatoId = candidatoId,
            NotaGeral = avaliacao.nota_geral,
            ComentarioRespostas = avaliacao.comentario_respostas.Trim(),
            ComentarioCurriculo = avaliacao.comentario_curriculo.Trim()
        });

        var dto = ParaDto(salva);
        dto.nomeCandidato ??= candidatura.Curriculo.Candidato.Nome;

        return dto;
    }

    public async Task<ListarAvaliacaoAgenteDto> Buscar(int vagaId, int candidatoId)
    {
        if (vagaId <= 0 || candidatoId <= 0)
            throw new DomainException("Informe IDs de vaga e candidato válidos.");

        var avaliacao = await _repository.BuscarPorCandidatoEVaga(candidatoId, vagaId)
            ?? throw new DomainException("Nenhuma avaliação encontrada para o candidato e vaga informados.");

        return ParaDto(avaliacao);
    }

    public async Task<List<ListarAvaliacaoAgenteDto>> ListarPorVaga(int vagaId)
    {
        if (vagaId <= 0)
            throw new DomainException("Informe um ID de vaga válido.");

        var avaliacoes = await _repository.ListarPorVaga(vagaId);

        if (avaliacoes == null || avaliacoes.Count == 0)
            throw new DomainException("Nenhuma avaliação encontrada para a vaga informada.");

        return avaliacoes.Select(ParaDto).ToList();
    }

    // ======================================================================
    // Helpers
    // ======================================================================

    private static Dictionary<string, object?> MontarRespostaPayload(VerificarQuestaoDto resposta)
    {
        var tipo = Enum.TryParse<TipoQuestaoEnum>(resposta.tipoQuestao, out var tipoEnum)
            ? NextAgentService.ConverterTipoQuestaoParaString(tipoEnum)
            : resposta.tipoQuestao;

        return new Dictionary<string, object?>
        {
            ["questao_id"] = resposta.questaoId,
            ["tipo_questao"] = tipo,
            ["status"] = resposta.acertou.HasValue
                ? (resposta.acertou.Value ? "acertou" : "errou")
                : null,
            ["resposta"] = resposta.acertou.HasValue
                ? null
                : string.Join(" ", resposta.respostasCandidato)
        };
    }

    /// <summary>
    /// Normaliza o currículo que vem do banco como jsonb (pode chegar
    /// double-encoded, com aspas escapadas) para enviar um objeto JSON ao agente.
    /// </summary>
    private static object? NormalizarCurriculo(string? curriculoJsonb)
    {
        if (string.IsNullOrWhiteSpace(curriculoJsonb))
            return null;

        try
        {
            var elemento = JsonSerializer.Deserialize<JsonElement>(curriculoJsonb);

            if (elemento.ValueKind == JsonValueKind.String)
            {
                var interno = elemento.GetString();
                if (!string.IsNullOrWhiteSpace(interno))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<JsonElement>(interno);
                    }
                    catch (JsonException)
                    {
                        return interno;
                    }
                }
            }

            return elemento;
        }
        catch (JsonException)
        {
            return curriculoJsonb;
        }
    }

    private static ListarAvaliacaoAgenteDto ParaDto(AvaliacaoAgente avaliacao)
        => new()
        {
            avaliacaoId = avaliacao.AvaliacaoId,
            vagaId = avaliacao.VagaId,
            candidatoId = avaliacao.CandidatoId,
            nomeCandidato = avaliacao.Candidato?.Nome,
            notaGeral = avaliacao.NotaGeral,
            comentarioRespostas = avaliacao.ComentarioRespostas,
            comentarioCurriculo = avaliacao.ComentarioCurriculo,
            dataAvaliacao = avaliacao.DataAvaliacao
        };

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
            return JsonSerializer.Deserialize<List<string>>(idiomasJson) ?? new List<string>();
        }
        catch (JsonException)
        {
            return new List<string>();
        }
    }
}

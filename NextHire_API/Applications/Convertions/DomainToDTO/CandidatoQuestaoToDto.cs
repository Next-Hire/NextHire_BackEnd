using System.Text.Json;
using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoQuestaoDTO;

namespace NextHire_API.Applications.Convertions.DomainToDTO;

public class CandidatoQuestaoToDto
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // Converte DTO -> Domain (Para o método Adicionar da Repository)
    public static CandidatoQuestao CriarCandidatoQuestao(CriarCandidatoQuestaoDTO dto)
    {
        return new CandidatoQuestao
        {
            QuestaoId = dto.questaoId,
            CandidatoId = dto.candidatoId,
            RepostaQuestao = JsonSerializer.Serialize(dto.respostaQuestao, _jsonOptions)
        };
    }

    // Converte Domain -> DTO (Para ListarPorCandidato, ListarPorQuestao, ListarCandidatoEVaga e BuscarPorId)
    public static LerCandidatoQuestaoDTO LerCandidatoQuestao(CandidatoQuestao candQuestao)
    {
        var payload = DeserializarResposta(candQuestao.RepostaQuestao);

        return new LerCandidatoQuestaoDTO
        {
            questaoId = candQuestao.QuestaoId,
            textoQuestao = candQuestao.Questao?.TextoQuestao ?? string.Empty,
            tipoQuestao = candQuestao.Questao?.TipoQuestao.ToString() ?? string.Empty,
            candidatoId = candQuestao.CandidatoId,
            nomeCandidato = candQuestao.Candidato?.Nome,
            respostaQuestao = payload,
            alternativas = candQuestao.Questao?.Alternativa?.Select(varAux => new AlternativaSemGabaritoDTO
            {
                alternativaId = varAux.AlternativaId,
                textoAlternativa = varAux.TextoAlternativa
            }).ToList() ?? new List<AlternativaSemGabaritoDTO>()
        };
    }

    // Converte Domain -> DTO com Gabarito e Correção (Para ListarRespostasComGabarito e ListaDeAcertos)
    public static LerCandidatoQuestaoGabaritoDTO LerComGabarito(CandidatoQuestao candQuestao)
    {
        var payload = DeserializarResposta(candQuestao.RepostaQuestao);

        return new LerCandidatoQuestaoGabaritoDTO
        {
            questaoId = candQuestao.QuestaoId,
            textoQuestao = candQuestao.Questao?.TextoQuestao ?? string.Empty,
            tipoQuestao = candQuestao.Questao?.TipoQuestao.ToString() ?? string.Empty,
            candidatoId = candQuestao.CandidatoId,
            respostaQuestao = payload,
            acertou = VerificarAcerto(candQuestao, payload),
            alternativas = candQuestao.Questao?.Alternativa?.Select(varAux => new AlternativaComGabaritoDTO
            {
                alternativaId = varAux.AlternativaId,
                textoAlternativa = varAux.TextoAlternativa,
                alternativaCorreta = varAux.AlternativaCorreta
            }).ToList() ?? new List<AlternativaComGabaritoDTO>()
        };
    }

    // desserializacao do JSON
    private static RespostaQuestaoPayloadDTO? DeserializarResposta(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        return JsonSerializer.Deserialize<RespostaQuestaoPayloadDTO>(json, _jsonOptions);
    }

    private static bool? VerificarAcerto(CandidatoQuestao candQuestao, RespostaQuestaoPayloadDTO? payload)
    {
        if (candQuestao.Questao == null || payload == null) return null;
        if (candQuestao.Questao.TipoQuestao == TipoQuestaoEnum.Dissertativa) return null;

        var textosCorretos = candQuestao.Questao.Alternativa
            .Where(a => a.AlternativaCorreta)
            .Select(a => a.TextoAlternativa.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        switch (candQuestao.Questao.TipoQuestao)
        {
            case TipoQuestaoEnum.VerdadeiroFalso:
                if (payload.resposta?.ValueKind == JsonValueKind.True)
                    return textosCorretos.Contains("Sim");
                if (payload.resposta?.ValueKind == JsonValueKind.False)
                    return textosCorretos.Contains("Não");
                return false;

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
}
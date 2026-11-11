using System.Text.Json;
using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoQuestaoDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services;

public class CandidatoQuestaoService
{
    private readonly ICandidato_QuestaoRepository _rep;

    public CandidatoQuestaoService(ICandidato_QuestaoRepository repository) => _rep = repository;


    public async Task<List<RespostaCandidatoQuestaoDTO>> ListarAcertos(int candidatoId)
    {
        var respostas =  await _rep.ListarRepostasCertas(candidatoId);
        if (respostas == null || respostas.Count == 0)
            throw new DomainException("Candidato não encontrado.");
        var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        if (opcoes == null)
            throw new DomainException("Alternativas não encontradas. ");
        
        var acertos = new List<RespostaCandidatoQuestaoDTO>();


        foreach (var resp in respostas)
        {
            if (resp.Questao.TipoQuestao == TipoQuestaoEnum.Dissertativa)
                continue;

            var payload = JsonSerializer.Deserialize<RespostaQuestaoPayload>(resp.RepostaQuestao);
            if (payload == null)
                continue;

            var respostasCertas = resp.Questao.Alternativa
                .Where(varAux => varAux.AlternativaCorreta)
                .Select(varAux => varAux.AlternativaId)
                .ToHashSet();
            
            if (respostasCertas.Count == 0 || respostasCertas == null)
                throw new DomainException("Candidato não acertou nenhuma.");

            var marcadas = payload.alternativaIds.ToHashSet();

            if (respostasCertas.SetEquals(marcadas))
            {
                acertos.Add(new RespostaCandidatoQuestaoDTO
                {
                    questaoId = resp.QuestaoId,
                    textoQuestao = resp.Questao.TextoQuestao
                });
            }
        }

        return acertos;

    }
}
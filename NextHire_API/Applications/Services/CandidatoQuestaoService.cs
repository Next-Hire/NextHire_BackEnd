using NextHire_API.Interfaces;
using NextHire_API.DTOs.CandidatoQuestaoDTO;
using NextHire_API.Applications.Convertions;
using NextHire_API.Applications.Convertions.DomainToDTO;
using NextHire_API.Domains;
using NextHire_API.Exceptions;

namespace NextHire_API.Applications.Services;


public class CandidatoQuestaoService
{
    private readonly ICandidato_QuestaoRepository _rep;
    public CandidatoQuestaoService(ICandidato_QuestaoRepository rep) => _rep = rep;
    
    public async Task<bool> ExisteRespostaCandidato(int questaoId, int candidatoId)
    {
        //
        // var Query = _rep.ExisteRespostaCandidato(questaoId, candidatoId);
        // if(Query == null ||  Query.IsFaulted)
        //     throw new DomainException("Candidato e/ou questao nao existe.");
        // return Query;
        return await _rep.ExisteRespostaCandidato(questaoId, candidatoId) 
               ?? throw new DomainException("Candidato e/ou questao nao existe.");
    }

    public async Task<LerCandidatoQuestaoDTO?> BuscarPorId(int questaoId, int candidatoId)
    {
        var resposta = await _rep.BuscarPorId(questaoId, candidatoId);
        if (resposta == null) 
            throw new DomainException("Candidato e/ou questão não encontrada. ");

        return CandidatoQuestaoToDto.LerCandidatoQuestao(resposta);
    }

    public async Task<List<LerCandidatoQuestaoDTO>> ListarPorCandidato(int candidatoId)
    {
        var lista = await _rep.ListarPorCandidato(candidatoId);
        if(lista == null || lista.Count == 0) 
            throw new DomainException("Candidato não encontrado.");
        return lista
            .Where(item => item != null)
            .Select(item => CandidatoQuestaoToDto.LerCandidatoQuestao(item!))
            .ToList();
    }

    public async Task<List<LerCandidatoQuestaoDTO>> ListarPorQuestao(int questaoId)
    {
        var lista = await _rep.ListarPorQuestao(questaoId);
        if (lista == null) 
            throw new DomainException("Candidato e/ou questão não encontrada. ");
        return lista
            .Where(item => item != null)
            .Select(item => CandidatoQuestaoToDto.LerCandidatoQuestao(item!))
            .ToList();
    }

    public async Task<List<LerCandidatoQuestaoDTO>> ListarCandidatoEVaga(int candidatoId, int vagaId)
    {
        var lista = await _rep.ListarCandidatoEVaga(candidatoId, vagaId);
        if (lista == null) 
            throw new DomainException("Candidato e/ou questão não encontrada. ");
        return lista
            .Select(CandidatoQuestaoToDto.LerCandidatoQuestao)
            .ToList();
    }

    public async Task<List<Questao?>> ListarPorQuestoesEmBranco(int candidatoId, int vagaId)
    {
        return await _rep.ListarPorQuestoesEmBranco(candidatoId, vagaId) 
               ?? throw new DomainException("Questões não encontradas.");
    }

    // public async Task<List<LerCandidatoQuestaoGabaritoDTO>> ListarRespostasComGabarito(int candidatoId, int? vagaId = null)
    // {
    //     var lista = await _rep.ListarRepostasCertas(candidatoId, vagaId);
    //     return lista
    //         .Where(item => item != null)
    //         .Select(item => CandidatoQuestaoToDto.LerComGabarito(item!))
    //         .ToList();
    // }

    public async Task<List<LerCandidatoQuestaoGabaritoDTO>> ListarRepostasCertas(int candidatoId)
    {
        var lista = await _rep.ListarRepostasCertas(candidatoId);
        if (lista == null) 
            throw new DomainException("Candidato não acertou nenhuma.. ");
        return lista
            .Select(CandidatoQuestaoToDto.LerComGabarito)
            .Where(dto => dto.acertou == true)
            .ToList();
    }

    // public async Task<List<LerCandidatoQuestaoGabaritoDTO>> ListaDeAcertos(int candidatoId, int vagaId)
    // {
    //     var lista = await _rep.ListarRepostasCertas(candidatoId, vagaId);
    //
    //     return lista
    //         .Select(CandidatoQuestaoToDto.LerComGabarito)
    //         .Where(dto => dto.acertou == true)
    //         .ToList();
    // }

    public async Task<LerCandidatoQuestaoDTO> Adicionar(CriarCandidatoQuestaoDTO dto)
    {
        var jaRespondeu = await _rep.ExisteRespostaCandidato(dto.questaoId, dto.candidatoId);
        if (jaRespondeu == true)
            throw new DomainException("O candidato já respondeu a esta questão.");

        var novaResposta = CandidatoQuestaoToDto.CriarCandidatoQuestao(dto);
        var salva = await _rep.Adicionar(novaResposta);

        var completa = await _rep.BuscarPorId(salva!.QuestaoId, salva.CandidatoId);
        if (completa == null)
            throw new DomainException("Questionário não encontrado. ");
        return CandidatoQuestaoToDto.LerCandidatoQuestao(completa);
    }

    public async Task<LerCandidatoQuestaoDTO?> Excluir(int candidatoId, int questaoId)
    {
        var excluida = await _rep.Excluir(candidatoId, questaoId);
        if (excluida == null) return null;

        return CandidatoQuestaoToDto.LerCandidatoQuestao(excluida);
    }
}
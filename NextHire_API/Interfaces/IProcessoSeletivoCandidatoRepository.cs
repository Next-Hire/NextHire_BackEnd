using NextHire_API.Domains;

namespace NextHire_API.Interfaces;

public interface IProcessoSeletivoCandidatoRepository
{
    public Task<List<CandidatoProcessoSeletivo>> Listar();

    public Task<List<CandidatoProcessoSeletivo>> ListarAprovados(int processoId);
    public Task<CandidatoProcessoSeletivo> BuscarPorId(int id, int processoSeletivoId);
    public Task<CandidatoProcessoSeletivo> BuscarPorCandidatoId(int id);
    public Task<CandidatoProcessoSeletivo> BuscarPorProcessoSeletivoId(int id);
    
    public Task<CandidatoProcessoSeletivo> AdicionarCandidatoProcesso(CandidatoProcessoSeletivo candProcesso);
    public Task<CandidatoProcessoSeletivo> AtualizarCandidatoProcesso(int candidatoId, int processoSeletivoId, CandidatoProcessoSeletivo candProcesso);
    public Task<CandidatoProcessoSeletivo> RemoverCandidatoProcesso(int id, int processoSeletivoId);


}
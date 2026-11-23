using NextHire_API.Domains;

namespace NextHire_API.Interfaces;

public interface IProcessoSeletivoCandidatoRepository
{
    public Task<List<CandidatoProcessoSeletivo>> Listar();

    public Task<List<CandidatoProcessoSeletivo>> ListarAprovados();
    public Task<CandidatoProcessoSeletivo> BuscarPorId(int id);
    public Task<CandidatoProcessoSeletivo> AdicionarCandidatoProcesso(CandidatoProcessoSeletivo candProcesso);
    public Task<CandidatoProcessoSeletivo> AtualizarCandidatoProcesso(CandidatoProcessoSeletivo candProcesso);
    public Task<CandidatoProcessoSeletivo> RemoverCandidatoProcesso(int id);


}
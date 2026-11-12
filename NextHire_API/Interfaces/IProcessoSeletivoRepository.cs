using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IProcessoSeletivoRepository
    {
        public Task<List<ProcessoSeletivo?>> Listar();

        public Task<List<ProcessoSeletivo?>> BuscarPorSetor();
        public Task<ProcessoSeletivo?> BuscarPorId(int processoSeletivoId);

    }
}

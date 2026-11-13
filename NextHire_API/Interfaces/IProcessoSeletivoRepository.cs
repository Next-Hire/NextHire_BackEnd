using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IProcessoSeletivoRepository
    {
        public Task<List<ProcessoSeletivo?>> Listar();

        public Task<List<ProcessoSeletivo?>> BuscarPorSetor();
        public Task<ProcessoSeletivo?> BuscarPorId(int processoSeletivoId);
        public Task<List<ProcessoSeletivo?>> BuscarPorSetor(int setorId, string? nomeSetor = null);
        public Task<List<ProcessoSeletivo?>> BuscarPorArea(int areaId, string? nomearea = null);
        public Task<ProcessoSeletivo?> BuscarPorVaga(int vagaId);
        public Task<ProcessoSeletivo?> AtualizarProcesso(int processoId, ProcessoSeletivo procSeletivo);
        public Task<ProcessoSeletivo?> DesativarProcesso(int processoId);



    }
}

using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IProcessoSeletivoRepository
    {
        Task<List<ProcessoSeletivo>> ListarProcessosSeletivos();
        Task<List<ProcessoSeletivo>> ListarProcessosSeletivosPorEmpresaId(int empresaId);
        Task<List<ProcessoSeletivo>> ListarProcessosSeletivosPorSetorId(int setorId);
        Task<List<ProcessoSeletivo>> ListarProcessosSeletivosPorAreaId(int areaId);
        Task<ProcessoSeletivo> BuscarProcessoSeletivoPorId(int id);
        Task<ProcessoSeletivo> AdicionarProcessoSeletivo(ProcessoSeletivo processoSeletivo);
        Task<ProcessoSeletivo> AtualizarProcessoSeletivo(ProcessoSeletivo processoSeletivo);
        Task<bool> RemoverProcessoSeletivo(int id);
    }
}
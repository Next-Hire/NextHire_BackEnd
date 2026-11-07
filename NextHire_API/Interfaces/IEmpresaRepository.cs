using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IEmpresaRepository
    {
        public Task<IEnumerable<Empresa?>> Listar();
        public Task<Empresa?> BuscarPorId(int? id);
        public Task<Empresa?> BuscarPorNome(string? nome);
        public Task<bool> EmpresaExiste(string nome);

        public Task<Empresa?> Adicionar(Empresa EmpresaNova);
        public Task<Empresa?> Atualizar(int id, Empresa EmpresaNova);
        public Task<Empresa?> Remover(int id);
    }
}

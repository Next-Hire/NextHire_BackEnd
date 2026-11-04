using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IEmpresaRepository
    {
        public Task<IEnumerable<empresa?>> Listar();
        public Task<empresa?> BuscarPorId(int id);
        public Task<empresa?> BuscarPorNome(string nome);
        public Task<bool?> empresaExiste(string nome);

        public Task<empresa?> Adicionar(empresa empresaNova);
        public Task<empresa?> Atualizar(int id, empresa empresaNova);
        public Task<empresa?> Remover(int id);
    }
}

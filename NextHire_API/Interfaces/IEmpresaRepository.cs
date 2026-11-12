using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IEmpresaRepository
    {
        public Task<IEnumerable<empresa>> Listar();
        public Task<empresa?> BuscarNome(string nome);
        public Task<empresa?> BuscarPorId(int empresaId);
        public Task<bool> empresaExiste(string nome);
        public Task<empresa?> Adicionar(empresa empresa);
        public Task<empresa?> Atualizar(int empresaId, empresa empresa);
        public Task<empresa> Remover(int empresaId);


    }
}

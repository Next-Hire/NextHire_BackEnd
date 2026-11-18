using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IVagaRepository
    {
        // Não é para ser consumido no frontend. é apenas para testes aqui no backend.
        public Task<List<Vaga>> Listar();

        public Task<List<Vaga>> ListarPorEmpresaId(int empresaId);

        public Task<List<Vaga>> ListarPorSetorId(int setorId);

        public Task<List<Vaga>> ListarPorTitulo(string titulo);

        public Task<List<Vaga>> ListarPorStatus(bool status);
        
        public Task<Vaga> BuscarPorId(int id);

        public Task Adicionar(Vaga vaga);

        public Task Atualizar(int vagaId, Vaga vaga);
        
        public Task Remover(int id);
    }
}
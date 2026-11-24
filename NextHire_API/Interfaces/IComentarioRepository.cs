using NextHire_API.Domains;

namespace NextHire_API.Applications.Services
{
    public interface IComentarioRepository
    {
        public Task<List<Comentario>> Listar();

        public Task<List<Comentario>> ListarPorEntrevistaId(int entrevistaId);

        public Task<List<Comentario>> ListarPorGestorId(int candidatoId);

        public Task<Comentario> BuscarPorId(int id);

        public Task Adicionar(Comentario comentario);

        public Task Atualizar(int comentarioId, Comentario comentario);
        
        public Task Remover(int id);
    }
}
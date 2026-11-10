using Microsoft.EntityFrameworkCore;
using NextHire_API.Applications.Services;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Exceptions;

namespace NextHire_API.Repository
{
    public class ComentarioRepository : IComentarioRepository
    {
        private readonly NextHireContext _context;

        public ComentarioRepository(NextHireContext context)
        {
            _context = context;
        }

        public async Task<List<Comentario>> Listar()
            => await _context.Comentario
                .Include(c => c.Entrevista)
                .Include(c => c.Gestor)
                .OrderByDescending(c => c.DataComentario)
                .ToListAsync();

        public async Task<List<Comentario>> ListarPorEntrevistaId(int entrevistaId)
            => await _context.Comentario
                .Include(c => c.Entrevista)
                .Include(c => c.Gestor)
                .OrderByDescending(c => c.DataComentario)
                .Where(c => c.EntrevistaId == entrevistaId).ToListAsync()
                    ?? throw new DomainException("Nenhum comentário encontrado para a entrevista informada.");

        public async Task<List<Comentario>> ListarPorGestorId(int gestorId)
            => await _context.Comentario
                .Include(c => c.Entrevista)
                .Include(c => c.Gestor)
                .OrderByDescending(c => c.DataComentario)
                .Where(c => c.GestorId == gestorId)
                .ToListAsync()
                    ?? throw new DomainException("Nenhum comentário encontrado para o gestor informado.");

        public async Task<Comentario> BuscarPorId(int id)
            => await _context.Comentario
                .Include(c => c.Entrevista)
                .Include(c => c.Gestor)
                .OrderByDescending(c => c.DataComentario)
                .FirstOrDefaultAsync(c => c.ComentarioId == id)
                    ?? throw new DomainException("Comentário não encontrado.");

        public async Task Adicionar(Comentario comentario)
        {
            await _context.Comentario.AddAsync(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(int comentarioId, Comentario comentario)
        {
            Comentario comentarioAtualizado = _context.Comentario.FirstOrDefault(c => c.ComentarioId == comentarioId)
                ?? throw new DomainException("Comentário não encontrado.");
            comentarioAtualizado.Texto = comentario.Texto;     

            _context.Comentario.Update(comentarioAtualizado);
            await _context.SaveChangesAsync();
        }

        public async Task Remover(int id)
        {
            var comentario = await _context.Comentario.FindAsync(id);
            if (comentario == null)
                throw new DomainException("Comentário não encontrado.");

            _context.Comentario.Remove(comentario);
            await _context.SaveChangesAsync();
        }
    }
}
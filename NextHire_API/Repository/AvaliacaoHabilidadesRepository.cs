using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class AvaliacaoHabilidadesRepository : IAvaliacaoHabilidadeRepository
    {
        private readonly NextHireContext _context;

        public AvaliacaoHabilidadesRepository(NextHireContext context)
        {
            _context = context;
        }

        public async Task<List<AvaliacaoHabilidade>> Listar()
        {
            return await _context.AvaliacaoHabilidade
                .AsNoTracking()
                .OrderBy(a => a.CandidatoId)
                .ThenBy(a => a.EntrevistaId)
                .ThenBy(a => a.HabilidadeId)
                .ToListAsync();
        }

        public async Task<AvaliacaoHabilidade?> BuscarPorChave(
            int candidatoId, int entrevistaId, int habilidadeId)
        {
            return await _context.AvaliacaoHabilidade.FirstOrDefaultAsync(a =>
                a.CandidatoId == candidatoId &&
                a.EntrevistaId == entrevistaId &&
                a.HabilidadeId == habilidadeId);
        }

        public async Task<List<AvaliacaoHabilidade>> BuscarPorCandidato(int candidatoId)
        {
            return await _context.AvaliacaoHabilidade
                .AsNoTracking()
                .Where(a => a.CandidatoId == candidatoId)
                .OrderBy(a => a.EntrevistaId)
                .ThenBy(a => a.HabilidadeId)
                .ToListAsync();
        }

        public async Task Adicionar(AvaliacaoHabilidade avaliacaoHabilidade)
        {
            if (avaliacaoHabilidade == null)
                throw new ArgumentNullException(nameof(avaliacaoHabilidade));

            await _context.AvaliacaoHabilidade.AddAsync(avaliacaoHabilidade);
            await _context.SaveChangesAsync();
        }

        public async Task<AvaliacaoHabilidade?> Atualizar(
            int candidatoId, int entrevistaId, int habilidadeId, int nivelAvaliado)
        {
            var avaliacaoExistente = await BuscarPorChave(candidatoId, entrevistaId, habilidadeId);
            if (avaliacaoExistente == null)
                return null;

            // Preserva todos os campos que compõem a chave primária.
            avaliacaoExistente.NivelAvaliado = nivelAvaliado;
            await _context.SaveChangesAsync();

            return avaliacaoExistente;
        }

        public async Task Deletar(int candidatoId, int entrevistaId, int habilidadeId)
        {
            var avaliacao = await BuscarPorChave(candidatoId, entrevistaId, habilidadeId);
            if (avaliacao == null)
                throw new KeyNotFoundException("Avaliação de habilidade não encontrada.");

            _context.AvaliacaoHabilidade.Remove(avaliacao);
            await _context.SaveChangesAsync();
        }
    }
}

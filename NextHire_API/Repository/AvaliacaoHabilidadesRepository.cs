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

        // Include recebe propriedades de navegação, não os campos FK (IDs).
        // A navegação da entrevista segue até o processo seletivo e a vaga.
        private IQueryable<AvaliacaoHabilidade> Consulta()
        {
            return _context.AvaliacaoHabilidade
                .Include(a => a.Candidato)
                .Include(a => a.Habilidade)
                .Include(a => a.Entrevista)
                    .ThenInclude(e => e.ProcessoSeletivo)
                        .ThenInclude(p => p.Vaga);
        }

        public async Task<List<AvaliacaoHabilidade>> Listar()
        {
            return await Consulta()
                .OrderBy(a => a.CandidatoId)
                .ThenBy(a => a.EntrevistaId)
                .ThenBy(a => a.HabilidadeId)
                .ToListAsync();
        }

        // Mantém tracking nesta consulta porque Atualizar e Deletar modificam a entidade.
        public async Task<AvaliacaoHabilidade?> BuscarPorId(
            int candidatoId, int entrevistaId, int habilidadeId)
        {
            return await Consulta().FirstOrDefaultAsync(a =>
                a.CandidatoId == candidatoId &&
                a.EntrevistaId == entrevistaId &&
                a.HabilidadeId == habilidadeId);
        }

        public async Task<List<AvaliacaoHabilidade>> BuscarPorCandidato(int candidatoId)
        {
            return await Consulta()
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
            var avaliacaoExistente = await BuscarPorId(candidatoId, entrevistaId, habilidadeId);
            if (avaliacaoExistente == null)
                return null;

            // Altera somente o campo não pertencente à chave composta.
            avaliacaoExistente.NivelAvaliado = nivelAvaliado;
            await _context.SaveChangesAsync();

            return avaliacaoExistente;
        }

        public async Task Deletar(int candidatoId, int entrevistaId, int habilidadeId)
        {
            var avaliacao = await BuscarPorId(candidatoId, entrevistaId, habilidadeId);
            if (avaliacao == null)
                throw new KeyNotFoundException("Avaliação de habilidade não encontrada.");

            _context.AvaliacaoHabilidade.Remove(avaliacao);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class AvaliacaoAgenteRepository : IAvaliacaoAgenteRepository
    {
        private readonly NextHireContext _context;

        public AvaliacaoAgenteRepository(NextHireContext context) => _context = context;

        public async Task<AvaliacaoAgente> Salvar(AvaliacaoAgente avaliacao)
        {
            var existente = await _context.AvaliacaoAgente
                .FirstOrDefaultAsync(a => a.VagaId == avaliacao.VagaId && a.CandidatoId == avaliacao.CandidatoId);

            if (existente != null)
            {
                existente.NotaGeral = avaliacao.NotaGeral;
                existente.ComentarioRespostas = avaliacao.ComentarioRespostas;
                existente.ComentarioCurriculo = avaliacao.ComentarioCurriculo;
                existente.DataAvaliacao = DateTime.Now;

                await _context.SaveChangesAsync();
                return existente;
            }

            avaliacao.DataAvaliacao = DateTime.Now;
            _context.AvaliacaoAgente.Add(avaliacao);
            await _context.SaveChangesAsync();
            return avaliacao;
        }

        public async Task<AvaliacaoAgente?> BuscarPorCandidatoEVaga(int candidatoId, int vagaId)
            => await _context.AvaliacaoAgente
                .Include(a => a.Candidato)
                .Include(a => a.Vaga)
                .FirstOrDefaultAsync(a => a.CandidatoId == candidatoId && a.VagaId == vagaId);

        public async Task<List<AvaliacaoAgente>> ListarPorVaga(int vagaId)
            => await _context.AvaliacaoAgente
                .Include(a => a.Candidato)
                .Include(a => a.Vaga)
                .Where(a => a.VagaId == vagaId)
                .OrderByDescending(a => a.NotaGeral)
                .ToListAsync();
    }
}

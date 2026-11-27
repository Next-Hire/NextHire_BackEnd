using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;
using System.Runtime.ExceptionServices;

namespace NextHire_API.Repository
{
    public class CurriculoVagaRepository : ICurriculoVagaRepository
    {
        private readonly NextHireContext _context;
        public CurriculoVagaRepository(NextHireContext context)
        {
            _context = context;
        }
        public async Task<List<CurriculoVaga>> ListarTodasAsCandidaturasPorEmpresaId(int empresaId)
        {
            var candidaturas = await _context.CurriculoVaga
                .Include(c => c.Curriculo)
                    .ThenInclude(c => c.Candidato)
                .Include(c => c.Vaga)
                .Where(c => c.Vaga.Empresa.EmpresaId == empresaId)
                .ToListAsync();

            if (candidaturas == null || candidaturas.Count <= 0)
                throw new DomainException("Nenhuma candidatura encontrada");

            return candidaturas;
        }

        public async Task<List<CurriculoVaga>> ListarCandidatosPorVagaId(int vagaId)
        {
            var candidaturas = await _context.CurriculoVaga
                .Include(c => c.Curriculo)
                    .ThenInclude(c => c.Candidato)
                .Include(c => c.Vaga)
                .Where(c => c.VagaId == vagaId)
                .ToListAsync();

            if (candidaturas == null || candidaturas.Count <= 0)
                throw new DomainException("Nenhuma candidatura encontrada");

            return candidaturas;
        }

        public async Task<List<Vaga>> ListarVagasPorCandidatoId(int candidatoId)
        {
            var vagas = await _context.Vaga
                .Include(v => v.AnalistaRhNavigation)
                .Include(v => v.Empresa)
                .Include(v => v.Setor)
                .Where(v => v.CurriculoVaga.Any(c => c.Curriculo.CandidatoId == candidatoId))
                .ToListAsync();

            return vagas;
        }

        public async Task CadastrarCurriculoVaga(int curriculoId, int vagaId)
        {
            var candidatura = new CurriculoVaga
            {
                CurriculoId = curriculoId,
                VagaId = vagaId
            };

            _context.CurriculoVaga.Add(candidatura);
            await _context.SaveChangesAsync();
        }

        public async Task Remover(int curriculoId, int vagaId)
        {
            var candidatura = await _context.CurriculoVaga
                .FirstOrDefaultAsync(c => c.CurriculoId == curriculoId && c.VagaId == vagaId);

            if (candidatura != null)
            {
                _context.CurriculoVaga.Remove(candidatura);
                await _context.SaveChangesAsync();
            }
        }
    }
}

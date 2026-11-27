using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class VagaRepository : IVagaRepository
    {
        private readonly NextHireContext _context;

        public VagaRepository(NextHireContext context)
        {
            _context = context;
        }

        //? Ordem de importância: abertas primeiro (fechadas no fim),
        //? prioridade alta -> media -> baixa, abertura mais recente primeiro,
        //? e data de fechamento como último desempate (fecha antes = mais urgente)
        private static readonly Func<IQueryable<Vaga>, IOrderedQueryable<Vaga>> OrdemImportancia = query
            => query
                .OrderByDescending(v => v.StatusVaga ?? false)
                .ThenBy(v => v.NivelPrioridadeVaga)
                .ThenByDescending(v => v.DataAbertura)
                .ThenBy(v => v.DataFechamento);

        public async Task<List<Vaga>> Listar()
            => await OrdemImportancia(_context.Vaga
                .Include(v => v.AnalistaRhNavigation)
                .Include(v => v.Empresa))
                .Include(v => v.Setor)
                .ToListAsync();

        public async Task<List<Vaga>> ListarPorEmpresaId(int empresaId)
            => await OrdemImportancia(_context.Vaga
                .Include(v => v.Empresa)
                .Include(v => v.AnalistaRhNavigation)
                .Where(v => v.EmpresaId == empresaId))
                .ToListAsync();
                
        public async Task<List<Vaga>> ListarPorSetorId(int setorId)
            => await OrdemImportancia(_context.Vaga
                .Include(v => v.Empresa)
                .Include(v => v.AnalistaRhNavigation)
                .Include(v => v.Setor)
                .Where(v => v.SetorId == setorId))
                .ToListAsync();

        public async Task<Vaga> BuscarPorId(int vagaId)
            => await _context.Vaga
                .Include(v => v.Empresa)
                .Include(v => v.AnalistaRhNavigation)
                .FirstOrDefaultAsync(v => v.VagaId == vagaId)
                    ?? throw new DomainException("Vaga não encontrada.");

        public async Task<List<Vaga>> ListarPorTitulo(string titulo)
            => await OrdemImportancia(_context.Vaga
                .Include(v => v.Empresa)
                .Include(v => v.AnalistaRhNavigation)
                .Where(v => v.Titulo.Contains(titulo)))
                .ToListAsync();

        public async Task<List<Vaga>> ListarPorStatus(bool status)
            => await OrdemImportancia(_context.Vaga
                .Include(v => v.Empresa)
                .Include(v => v.AnalistaRhNavigation)
                .Where(v => v.StatusVaga == status))
                .ToListAsync();

        public async Task Adicionar(Vaga vaga)
        {
            await _context.Vaga.AddAsync(vaga);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(int vagaId, Vaga vaga)
        {
            var vagaExistente = await _context.Vaga.FindAsync(vagaId)
                ?? throw new DomainException("Vaga não encontrada.");

            vagaExistente.AnalistaRh = vaga.AnalistaRh;
            vagaExistente.SetorId = vaga.SetorId;
            vagaExistente.DataFechamento = vaga.DataFechamento;
            vagaExistente.Descricao = vaga.Descricao;
            vagaExistente.EmpresaId = vaga.EmpresaId;
            vagaExistente.IdiomasDesejados = vaga.IdiomasDesejados;
            vagaExistente.Remuneracao = vaga.Remuneracao;
            vagaExistente.NivelPrioridadeVaga = vaga.NivelPrioridadeVaga;
            vagaExistente.TipoVaga = vaga.TipoVaga;
            vagaExistente.TempoExperienciaDesejado = vaga.TempoExperienciaDesejado;
            vagaExistente.Titulo = vaga.Titulo;

            if (!_context.Entry(vagaExistente).Properties.Any(p => p.IsModified))
                throw new DomainException("Nenhuma alteração foi feita na vaga.");

            await _context.SaveChangesAsync();
        }

        public async Task Remover(int id)
        {
            var vaga = await _context.Vaga.FindAsync(id);
            if (vaga == null)
                throw new DomainException("Vaga não encontrada.");

            _context.Vaga.Remove(vaga);
            await _context.SaveChangesAsync();
        }
    }
}
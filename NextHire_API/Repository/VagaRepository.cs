using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
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

        public async Task<List<Vaga>> Listar()
            => await _context.Vaga
                .Include(v => v.AnalistaRhNavigation)
                .Include(v => v.Empresa)
                .OrderBy(v => v.DataAbertura)
                .ToListAsync();

        public async Task<List<Vaga>> ListarPorEmpresaId(int empresaId)
            => await _context.Vaga
                .Include(v => v.Empresa)
                .OrderBy(v => v.DataAbertura)
                .Where(v => v.EmpresaId == empresaId)
                .ToListAsync();

        public async Task<Vaga> BuscarPorId(int vagaId)
            => await _context.Vaga
                .Include(v => v.Empresa)
                .Include(v => v.AnalistaRhNavigation)
                .OrderBy(v => v.DataAbertura)
                .FirstOrDefaultAsync(v => v.VagaId == vagaId)
                    ?? throw new Exception("Vaga não encontrada.");

        public async Task<List<Vaga>> ListarPorTitulo(string titulo)
            => await _context.Vaga
                .Include(v => v.Empresa)
                .Include(v => v.AnalistaRhNavigation)
                .OrderBy(v => v.DataAbertura)
                .Where(v => v.Titulo.Contains(titulo))
                .ToListAsync();

        public async Task<List<Vaga>> ListarPorStatus(bool status)
            => await _context.Vaga
                .Include(v => v.Empresa)
                .Include(v => v.AnalistaRhNavigation)
                .OrderBy(v => v.DataAbertura)
                .Where(v => v.StatusVaga == status)
                .ToListAsync();

        public async Task Adicionar(Vaga vaga)
        {
            await _context.Vaga.AddAsync(vaga);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(int vagaId, Vaga vaga)
        {
            var vagaExistente = await _context.Vaga.FindAsync(vagaId)
                ?? throw new Exception("Vaga não encontrada.");

            vagaExistente.AnalistaRh = vaga.AnalistaRh;
            vagaExistente.DataFechamento = vaga.DataFechamento;
            vagaExistente.Descricao = vaga.Descricao;
            vagaExistente.EmpresaId = vaga.EmpresaId;
            vagaExistente.IdiomasDesejados = vaga.IdiomasDesejados;
            vagaExistente.TempoExperienciaDesejado = vaga.TempoExperienciaDesejado;
            vagaExistente.Titulo = vaga.Titulo;

            if (!_context.Entry(vagaExistente).Properties.Any(p => p.IsModified))
                throw new Exception("Nenhuma alteração foi feita na vaga.");

            await _context.SaveChangesAsync();
        }

        public async Task Remover(int id)
        {
            var vaga = await _context.Vaga.FindAsync(id);
            if (vaga == null)
                throw new Exception("Vaga não encontrada.");

            _context.Vaga.Remove(vaga);
            await _context.SaveChangesAsync();
        }
    }
}
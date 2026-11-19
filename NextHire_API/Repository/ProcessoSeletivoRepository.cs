using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class ProcessoSeletivoRepository : IProcessoSeletivoRepository
    {
        private readonly NextHireContext _context;
        public ProcessoSeletivoRepository(NextHireContext context)
            => _context = context;

        public async Task<List<ProcessoSeletivo>> ListarProcessosSeletivos()
            => await _context.ProcessoSeletivo
            .Include(p => p.Vaga)
            .ThenInclude(p => p.Empresa)
            .ThenInclude(p => p.SetorOcupacao)
            .ThenInclude(p => p.AreaOcupacao)
            .ToListAsync();
        
        public async Task<List<ProcessoSeletivo>> ListarProcessosSeletivosPorEmpresaId(int empresaId)
            => await _context.ProcessoSeletivo
            .Include(p => p.Vaga)
            .ThenInclude(p => p.Empresa)
            .ThenInclude(p => p.SetorOcupacao)
            .ThenInclude(p => p.AreaOcupacao)
            .Where(p => p.Vaga.EmpresaId == empresaId)
            .ToListAsync();

        public async Task<List<ProcessoSeletivo>> ListarProcessosSeletivosPorSetorId(int setorId)
            => await _context.ProcessoSeletivo
            .Include(p => p.Vaga)
            .ThenInclude(p => p.Empresa)
            .ThenInclude(p => p.SetorOcupacao)
            .ThenInclude(p => p.AreaOcupacao)
            .Where(p => p.Vaga.SetorId == setorId)
            .ToListAsync();

        public async Task<List<ProcessoSeletivo>> ListarProcessosSeletivosPorAreaId(int areaId)
            => await _context.ProcessoSeletivo
            .Include(p => p.Vaga)
            .ThenInclude(p => p.Empresa)
            .ThenInclude(p => p.SetorOcupacao)
            .ThenInclude(p => p.AreaOcupacao)
            .Where(p => p.Vaga.Setor.AreaOcupacaoId == areaId)
            .ToListAsync();

        public async Task<ProcessoSeletivo> BuscarProcessoSeletivoPorId(int processoId)
            => await _context.ProcessoSeletivo
            .Include(p => p.Vaga)
            .ThenInclude(p => p.Empresa)
            .ThenInclude(p => p.SetorOcupacao)
            .ThenInclude(p => p.AreaOcupacao)
            .FirstOrDefaultAsync(p => p.ProcessoSeletivoId == processoId) 
            ?? throw new Exception("Processo seletivo não encontrado");

        public async Task<ProcessoSeletivo> AdicionarProcessoSeletivo(ProcessoSeletivo processoSeletivo)
        {
            _context.ProcessoSeletivo.Add(processoSeletivo);
            await _context.SaveChangesAsync();
            return processoSeletivo;
        }

        public async Task<ProcessoSeletivo> AtualizarProcessoSeletivo(ProcessoSeletivo processoSeletivo)
        {
            _context.ProcessoSeletivo.Update(processoSeletivo);
            await _context.SaveChangesAsync();
            return processoSeletivo;
        }
        
        public async Task<bool> RemoverProcessoSeletivo(int processoId)
        {
            var processoSeletivo = await _context.ProcessoSeletivo.FindAsync(processoId);
            if (processoSeletivo == null)
                return false;

            processoSeletivo.StatusProcessoSeletivo = false;

            _context.ProcessoSeletivo.Update(processoSeletivo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
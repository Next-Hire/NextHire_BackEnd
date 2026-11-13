using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class ProcessoSeletivoRepository : IProcessoSeletivoRepository
    {
        private readonly NextHireContext _ctx;

        public ProcessoSeletivoRepository(NextHireContext ctx) => _ctx = ctx;

        public async Task<List<ProcessoSeletivo?>> Listar()
            => await _ctx.ProcessoSeletivo
            .Include(varAux => varAux.Vaga)
            .Include(varAux => varAux.Candidato)

            .ToListAsync();

        public async Task<ProcessoSeletivo?> BuscarPorId(int pSeletivoId)
            => await _ctx.ProcessoSeletivo
            .Include(varAux => varAux.Vaga)
            .Include(varAux => varAux.Candidato)
            .FirstOrDefaultAsync(varAux => varAux.ProcessoSeletivoId == pSeletivoId);


    }
}

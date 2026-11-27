using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class TipoEntrevistaRepository : ITipoEntrevistaRepository
    {

        private readonly NextHireContext _ctx;

        public TipoEntrevistaRepository(NextHireContext ctx) => _ctx = ctx;


        public async Task<List<TipoEntrevista>> Listar()
            =>  await _ctx.TipoEntrevista
                    .AsNoTracking()
                    .ToListAsync();

        public async Task<TipoEntrevista?> BuscarPorTipo(string tipo)
            => await _ctx.TipoEntrevista
                    .AsNoTracking()
                    .FirstOrDefaultAsync(varAux => varAux.Nome_Tipo == tipo);

        public async Task<TipoEntrevista?> BuscarPorId(int id)
            => await _ctx.TipoEntrevista.FindAsync(id);
    }
}

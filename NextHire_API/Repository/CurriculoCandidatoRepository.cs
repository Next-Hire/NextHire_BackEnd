using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class CurriculoCandidatoRepository : ICurriculoCandidatoRepository
    {
        private readonly NextHireContext _ctx;

        public CurriculoCandidatoRepository(NextHireContext ctx)
         =>    _ctx = ctx;

        public async Task<List<CurriculoCandidato>> ListarCurriculos()
         =>   await _ctx.CurriculoCandidato
                .Include(varAux => varAux.Candidato)
                //.Include(varAux =>varAux.CurriculoVaga)
                //    .ThenInclude(varAux => varAux.Select(varAux => varAux.))
                .ToListAsync();

        public async Task<bool> existeCurriculo(int curriculoId)
            => await _ctx.CurriculoCandidato.AnyAsync(varAux => varAux.CurriculoCandidatoId == curriculoId);
        public async Task<CurriculoCandidato?> BuscarCurriculoPorId(int curriculoId) => 
            await _ctx.CurriculoCandidato
                .Include(varAux => varAux.Candidato)
                .FirstOrDefaultAsync(varAux => varAux.CurriculoCandidatoId == curriculoId);

        public async Task<CurriculoCandidato?> BuscarCurriculoPorCandidatoId(int candidatoId)
            => await _ctx.CurriculoCandidato
            .Include(varAux => varAux.Candidato)
            .FirstOrDefaultAsync(varAux => varAux.CandidatoId == candidatoId);

        public async Task<Usuario?> buscarCandidatoId(int candidatoId)
            => await _ctx.Usuario.FindAsync(candidatoId);

        public async Task<CurriculoCandidato> AdicionarCurriculo(CurriculoCandidato curriculoCandidato)
        {
            await _ctx.CurriculoCandidato.AddAsync(curriculoCandidato);
            await _ctx.SaveChangesAsync();

            return curriculoCandidato;
        }

        public async Task<CurriculoCandidato?> RemoverCurriculo(int curriculoCandidatoId)
        {
            CurriculoCandidato? curriculoQuery = await _ctx.CurriculoCandidato.FindAsync(curriculoCandidatoId);
            if (curriculoQuery == null)
                return null;


            _ctx.CurriculoCandidato.Remove(curriculoQuery);
            await _ctx.SaveChangesAsync();

            return curriculoQuery;
            
        }
        
        
    }
}

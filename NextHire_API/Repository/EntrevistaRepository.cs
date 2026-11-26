using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public class EntrevistaRepository : IEntrevistaRepository
    {
        private readonly NextHireContext _ctx;
        public EntrevistaRepository(NextHireContext ctx) => _ctx = ctx;

        public async Task<List<Entrevista>> Listar()
            => await _ctx.Entrevista
                .AsNoTracking()
                .Include(varAux => varAux.Comentario)
            .Where(varAux => varAux.HorarioInicio >= DateTime.Now)
            .OrderBy(varAux => varAux.HorarioInicio)
            .ToListAsync();


        public async Task<List<Entrevista>> EntrevistasHoje(DateTime data)
            {
            DateTime inicioDoDia = data.Date;
            DateTime fimDoDia = inicioDoDia.AddDays(1);

            List<Entrevista> entrevistasHoje = await _ctx.Entrevista
            .AsNoTracking()
            .Include(varAux => varAux.Comentario)
            .Where(varAux => varAux.HorarioInicio >= inicioDoDia && varAux.HorarioInicio < fimDoDia)
            .OrderBy(varAux => varAux.HorarioInicio)
            .ToListAsync();

            return entrevistasHoje;
            
            }


        public async Task<List<Entrevista>> BuscarPorCandidato(int candidatoId)
        {
            List<Entrevista> entrevistaQuery = await _ctx.Entrevista
             .AsNoTracking()
             .Include(varAux => varAux.Comentario)
             .Include(varAux => varAux.Candidato)
                 .ThenInclude(varAux => varAux.InformacaoCandidato)
             .Where(varAux => varAux.HorarioInicio >= DateTime.UtcNow && varAux.Candidato.Any(varAux => varAux.UsuarioId == candidatoId))
           .OrderBy(varAux => varAux.HorarioInicio)
           .ToListAsync();

            return entrevistaQuery;
        }


        public async Task<Entrevista?> BuscarPorId(int id)
            => await _ctx.Entrevista.FindAsync(id);

        public async Task<Entrevista> AdicionarEntrevista(Entrevista entr, List<int> candidatosIds)
        {
            if (candidatosIds != null && candidatosIds.Any())
            {
                List<Usuario> candidatos = await _ctx.Usuario
                    .Include(u => u.InformacaoCandidato) // <-- Garante que InformacaoCandidato não venha null
                    .Where(u => candidatosIds.Contains(u.UsuarioId) && u.TipoUsuario == TipoUsuarioEnum.Candidato)
                    .ToListAsync();

                foreach (Usuario candidato in candidatos)
                {
                    entr.Candidato.Add(candidato);
                }
            }

            await _ctx.Entrevista.AddAsync(entr);
            await _ctx.SaveChangesAsync();

            return entr;
        }


        public async Task<Entrevista> AtualizarCandidatoNaEntrevista(int entrevistaId, int candidatoId)
        {
            Entrevista? entrQuery = await _ctx.Entrevista.FindAsync(entrevistaId);

            Usuario? usuarioQuery = await _ctx.Usuario
                                    .Where(varAux => varAux.TipoUsuario == TipoUsuarioEnum.Candidato)
                                    .FirstOrDefaultAsync(varAux => varAux.UsuarioId == candidatoId);

            if (entrQuery == null || usuarioQuery == null)
                return null;

            entrQuery.Candidato.Clear();
            entrQuery.Candidato.Add(usuarioQuery);

            await _ctx.SaveChangesAsync();

            return entrQuery;

            
        }
   
        public async Task<Entrevista?> AtualizarDataDaEntrevista(int entrevistaId, DateTime? dataInicio, DateTime? dataFinal, string? link = null)
        {
            Entrevista? entrQuery = await _ctx.Entrevista
                .Include(e => e.Candidato)
                .ThenInclude(c => c.InformacaoCandidato)
                .FirstOrDefaultAsync(e => e.EntrevistaId == entrevistaId);

            if (entrQuery == null)
                return null;

            if (dataInicio.HasValue)
                entrQuery.HorarioInicio = dataInicio.Value;

            if (dataFinal.HasValue)
                entrQuery.HorarioFim = dataFinal.Value;

            if (link != null)
                entrQuery.Link = string.IsNullOrWhiteSpace(link) ? null : link.Trim();

            // Se HorarioInicio for DateTime? (nullable), use .GetValueOrDefault() ou .Value:
            DateTime inicioEfetivo = entrQuery.HorarioInicio;
            if (entrQuery.HorarioFim <= inicioEfetivo)
                return null;

            await _ctx.SaveChangesAsync();

            return entrQuery;
        }

        public async Task<Entrevista> RemoverEntrevista(int id)
        {
            Entrevista? entrQuery = await _ctx.Entrevista.FindAsync(id);
            if (entrQuery == null)
                return null;

            _ctx.Entrevista.Remove(entrQuery);
            await _ctx.SaveChangesAsync();

            return entrQuery;

        }

        public async Task<List<Entrevista>> ListarPorStatus(bool status)
        {
            List<Entrevista> entrevistasAtivas = await _ctx.Entrevista
                .AsNoTracking()
                .Include(varAux => varAux.Comentario)
            .Where(varAux => varAux.StatusEntrevista == status)
            .OrderBy(varAux => varAux.HorarioInicio)
            .ToListAsync();

            return entrevistasAtivas;
        }
    }
}

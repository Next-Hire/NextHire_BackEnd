using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class ExperienciaProfissionalRepository : IExperienciaProfissionalRepository
    {
        private readonly NextHireContext _context;
        public ExperienciaProfissionalRepository(NextHireContext context)
        {
            _context = context;
        }  

        public async Task<List<experiencia_profissional>> ListarExperienciasProfissionaisPorUsuario(int idUsuario, bool? ativo)
        {
            List<experiencia_profissional> experienciasProfissionais = 
                    await _context.experiencia_profissional
                        .Include(e => e.usuario)
                        .OrderBy(e => e.data_entrada)
                        .Where(e => e.usuario_id == idUsuario)
                        .ToListAsync();

            if(ativo == true)
                return experienciasProfissionais
                    .Where(e => e.atualmente_trabalho == true)
                    .ToList();

            if(ativo == false)
                return experienciasProfissionais
                    .Where(e => e.atualmente_trabalho == false)
                    .ToList();

            return experienciasProfissionais;
        }

        public async Task<experiencia_profissional> BuscarExperienciaProfissionalPorId(int idExperienciaProfissional) => 
                await _context.experiencia_profissional
                    .FirstOrDefaultAsync(e => e.experiencia_profissional_id == idExperienciaProfissional);

        public async Task CadastrarExperienciaProfissional(experiencia_profissional experienciaProfissional)
        {
            await _context.experiencia_profissional.AddAsync(experienciaProfissional);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarExperienciaProfissional(int idExperienciaProfissional, experiencia_profissional experienciaProfissional)
        {
            experiencia_profissional experienciaProfissionalBuscada = 
                await _context.experiencia_profissional
                    .FirstOrDefaultAsync(e => e.experiencia_profissional_id == idExperienciaProfissional) 
                        ?? throw new InvalidOperationException("Experiencia profissional nao encontrada.");
            
            // METODO NOVO PARA USAR DE UPDATE
            _context.Entry(experienciaProfissionalBuscada).CurrentValues.SetValues(experienciaProfissional);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverExperienciaProfissional(int idExperienciaProfissional)
        {
            experiencia_profissional experienciaProfissionalBuscada = 
                await _context.experiencia_profissional
                    .FirstOrDefaultAsync(e => e.experiencia_profissional_id == idExperienciaProfissional) 
                        ?? throw new InvalidOperationException("Experiencia profissional nao encontrada.");

            _context.experiencia_profissional.Remove(experienciaProfissionalBuscada);
            await _context.SaveChangesAsync();
        }
    }
}

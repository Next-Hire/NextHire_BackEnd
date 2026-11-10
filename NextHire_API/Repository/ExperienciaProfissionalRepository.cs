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

        public async Task<List<ExperienciaProfissional>> ListarExperienciasProfissionaisPorUsuario(int idUsuario, bool? ativo)
        {
            List<ExperienciaProfissional> experienciasProfissionais = 
                    await _context.ExperienciaProfissional
                        .Include(e => e.Usuario)
                        .OrderBy(e => e.DataEntrada)
                        .Where(e => e.UsuarioId == idUsuario)
                        .ToListAsync();

            if(ativo == true)
                return experienciasProfissionais
                    .Where(e => e.AtualmenteTrabalho == true)
                    .ToList();

            if(ativo == false)
                return experienciasProfissionais
                    .Where(e => e.AtualmenteTrabalho == false)
                    .ToList();

            return experienciasProfissionais;
        }

        public async Task<ExperienciaProfissional> BuscarExperienciaProfissionalPorId(int idExperienciaProfissional) => 
                await _context.ExperienciaProfissional
                    .FirstOrDefaultAsync(e => e.ExperienciaProfissionalId == idExperienciaProfissional) 
                        ?? throw new InvalidOperationException("Experiencia profissional não encontrada.");

        public async Task CadastrarExperienciaProfissional(ExperienciaProfissional experienciaProfissional)
        {
            await _context.ExperienciaProfissional.AddAsync(experienciaProfissional);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarExperienciaProfissional(int idExperienciaProfissional, ExperienciaProfissional experienciaProfissional)
        {
            ExperienciaProfissional experienciaProfissionalBuscada = 
                await _context.ExperienciaProfissional
                    .FirstOrDefaultAsync(e => e.ExperienciaProfissionalId == idExperienciaProfissional) 
                        ?? throw new InvalidOperationException("Experiencia profissional nao encontrada.");
            
            // METODO NOVO PARA USAR DE UPDATE
            _context.Entry(experienciaProfissionalBuscada).CurrentValues.SetValues(experienciaProfissional);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverExperienciaProfissional(int idExperienciaProfissional)
        {
            ExperienciaProfissional experienciaProfissionalBuscada = 
                await _context.ExperienciaProfissional
                    .FirstOrDefaultAsync(e => e.ExperienciaProfissionalId == idExperienciaProfissional) 
                        ?? throw new InvalidOperationException("Experiencia profissional nao encontrada.");

            _context.ExperienciaProfissional.Remove(experienciaProfissionalBuscada);
            await _context.SaveChangesAsync();
        }
    }
}

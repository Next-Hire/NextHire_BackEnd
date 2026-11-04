using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IExperienciaProfissionalRepository
    {
        public Task<List<experiencia_profissional>> ListarExperienciasProfissionaisPorUsuario(int idUsuario, bool? ativo);
        public Task<experiencia_profissional> BuscarExperienciaProfissionalPorId(int idExperienciaProfissional);
        public Task CadastrarExperienciaProfissional(experiencia_profissional experienciaProfissional);
        public Task AtualizarExperienciaProfissional(int idExperienciaProfissional, experiencia_profissional experienciaProfissional);
        public Task RemoverExperienciaProfissional(int idExperienciaProfissional);
    }
}

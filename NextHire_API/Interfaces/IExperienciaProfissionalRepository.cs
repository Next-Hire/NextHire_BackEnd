using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IExperienciaProfissionalRepository
    {
        public Task<List<ExperienciaProfissional>> ListarExperienciasProfissionaisPorUsuario(int idUsuario, bool? ativo);
        public Task<ExperienciaProfissional> BuscarExperienciaProfissionalPorId(int idExperienciaProfissional);
        public Task CadastrarExperienciaProfissional(ExperienciaProfissional experienciaProfissional);
        public Task AtualizarExperienciaProfissional(int idExperienciaProfissional, ExperienciaProfissional experienciaProfissional);
        public Task RemoverExperienciaProfissional(int idExperienciaProfissional);
    }
}

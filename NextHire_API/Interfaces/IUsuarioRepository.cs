using NextHire_API.Domains;

namespace NextHireAPI.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<List<Usuario>> ListarFuncionariosPorEmpresaId(int empresaId);

        public Task<Usuario> BuscarFuncionarioPorId(int funcionarioId);
        public Task<Usuario> BuscarCandidatoPorId(int candidatoId);

        //? Optei por adicionar a configuracao do id da empresa na service.
        public Task AdicionarFuncionarios(List<Usuario> usuarios);

        public Task CadastrarCandidato(Usuario usuario);

        public Task AtualizarUsuarioGeral(Usuario usuarioBuscado, Usuario usuarioAtualizado);

        public Task AtualizarSenha(int usuarioid, string senha);
    }
}
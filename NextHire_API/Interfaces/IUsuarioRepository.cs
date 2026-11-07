using Microsoft.OpenApi;
using NextHire_API.Domains;

namespace NextHireAPI.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<List<Usuario>> ListarUsuarios();
        public Task<List<Usuario>> ListarFuncionariosPorEmpresaId(int empresaId);

        public Task<List<Usuario>> ListarCandidatos();

        public Task<Usuario> BuscarUsuarioPorEmail(string email);

        public Task<Usuario> BuscarFuncionarioPorId(int funcionarioId);

        public Task<Usuario> BuscarCandidatoPorId(int candidatoId);

        //? Optei por adicionar a configuracao do id da empresa na service.
        public Task AdicionarFuncionarios(int empresaId, List<Usuario> funcionarios);

        public Task CadastrarCandidato(Usuario usuario);

        public Task AtualizarUsuarioGeral(int usuairoId, Usuario usuarioAtualizado);

        public Task AtualizarSenha(int usuarioid, string senha);

        public Task RemoverUsuario(Usuario usuario);
    }
}
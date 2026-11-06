using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHireAPI.Interfaces;

namespace NextHireAPI.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly NextHireContext _context;
        public UsuarioRepository(NextHireContext context) => _context = context;

        public async Task<List<Usuario>> ListarFuncionariosPorEmpresaId(int empresaId)
        {
            Empresa empresa = await _context.Empresa.FirstAsync(e => e.EmpresaId == empresaId);

            return empresa.Funcionario.ToList();
        }

        public async Task<Usuario> BuscarFuncionarioPorId(int funcionarioId)
            => await _context.Usuario.FirstOrDefaultAsync(f => f.UsuarioId == funcionarioId);

        public async Task<Usuario> BuscarCandidatoPorId(int candidatoId)
            => await _context.Usuario
                .Include(u => u.AnotacaoCandidato)
                .Include(u => u.AnotacaoGestor)
                .Include(u => u.AreaOcupacao)
                .Include(u => u.CandidatoQuestao)
                .Include(u => u.CandidatoVaga)
                .Include(u => u.CurriculoCandidato)
                .FirstOrDefaultAsync(u => u.UsuarioId == candidatoId);

        public async Task AdicionarFuncionarios(List<Usuario> usuarios)
            => usuarios.Select(async u => await _context.Usuario.AddAsync(u));

        public async Task CadastrarCandidato(Usuario usuario)
            => await _context.Usuario.AddAsync(usuario);

        public async Task AtualizarUsuarioGeral(Usuario usuarioBuscado, Usuario usuarioAtualizado)
            => _context.Entry(usuarioBuscado).CurrentValues.SetValues(usuarioAtualizado);

        public async Task AtualizarSenha(int usuarioId, string senha)
        {
            Usuario usuarioBuscado = await _context.Usuario.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

            usuarioBuscado.Senha = senha;
            
            if (usuarioBuscado.PrimeiroAcesso == true)
                usuarioBuscado.PrimeiroAcesso = false;

            _context.Usuario.Update(usuarioBuscado);
            await _context.SaveChangesAsync();
        }
    }
}
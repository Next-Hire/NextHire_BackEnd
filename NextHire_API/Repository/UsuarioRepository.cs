using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHireAPI.Applications.Services;
using NextHireAPI.Interfaces;

namespace NextHireAPI.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly NextHireContext _context;
        public UsuarioRepository(NextHireContext context) => _context = context;

        public async Task<List<Usuario>> ListarUsuarios()
            => await _context.Usuario
                .ToListAsync()
                ?? throw new DomainException($"Nenhum usuario localizado");


        public async Task<List<Usuario>> ListarFuncionariosPorEmpresaId(int empresaId)
        {
            Empresa empresa = await _context.Empresa
                .Include(e => e.Funcionario)
                .FirstOrDefaultAsync(e => e.EmpresaId == empresaId)
                ?? throw new DomainException("Empresao não encontrada.");


            return empresa.Funcionario.ToList();
        }

        public async Task<List<Usuario>> ListarCandidatos()
            => await _context.Usuario
                .Where(u => u.TipoUsuario == TipoUsuarioEnum.Candidato)
                .Include(varAux => varAux.ExperienciaProfissional)
                .Include(varAux => varAux.HabilidadeCandidato)
                .Include(varAux => varAux.InformacaoCandidato)
                .ToListAsync();

        public async Task<Usuario> BuscarFuncionarioPorId(int funcionarioId)
        {
            Usuario funcionario = await _context.Usuario
                .Include(f => f.Empresa)
                .FirstOrDefaultAsync(f => f.UsuarioId == funcionarioId);

            if (funcionario == null || funcionario.Empresa == null)
                throw new DomainException($"O usuario com o ID: {funcionarioId} não foi localizado.");

            return funcionario;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string email)
            => await _context.Usuario
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new DomainException($"Usuario não localizado,");

        public async Task<Usuario> BuscarCandidatoPorId(int candidatoId)
            => await _context.Usuario
                .Include(u => u.AnotacaoCandidato)
                .Include(u => u.AnotacaoGestor)
                .Include(u => u.AreaOcupacao)
                .Include(u => u.CandidatoQuestao)
                .Include(u => u.CandidatoVaga)
                .Include(u => u.CurriculoCandidato)
                .FirstOrDefaultAsync(u => u.UsuarioId == candidatoId)
                ?? throw new DomainException($"O candidato com o ID: {candidatoId} não foi localizado,");

        public async Task AdicionarFuncionarios(int empresaId, List<Usuario> funcionarios)
        {
            Empresa empresa = _context.Empresa
                .FirstOrDefault(e => e.EmpresaId == empresaId)
                    ?? throw new DomainException("Empresa não encontrada.");

            foreach (var funcionario in funcionarios)
            {
                if (!EmailService.ValidarEmail(funcionario.Email))
                    throw new DomainException($"O email: {funcionario.Email} é inválido.");

                await _context.Usuario.AddAsync(funcionario);
                empresa.Funcionario.Add(funcionario);
            }

            var registros = await _context.SaveChangesAsync();
            Console.WriteLine($"6 - Registros afetados: {registros}");
        }

        public async Task CadastrarCandidato(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarUsuarioGeral(int usuarioId, Usuario usuarioAtualizado)
        {
            Usuario usuarioBuscado = await _context.Usuario.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId)
                ?? throw new DomainException("Usuario não localizado.");

            if (usuarioAtualizado.Nome != null)
                usuarioBuscado.Nome = usuarioAtualizado.Nome;

            if (usuarioAtualizado.Email != null)
                usuarioBuscado.Email = usuarioAtualizado.Email;

            _context.Usuario.Update(usuarioBuscado);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarSenha(int usuarioId, string senha)
        {
            Usuario usuarioBuscado = await _context.Usuario.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

            if (usuarioBuscado == null)
                throw new DomainException("USuario nao encontrado");

            usuarioBuscado.Senha = senha;

            if (usuarioBuscado.PrimeiroAcesso == true)
                usuarioBuscado.PrimeiroAcesso = false;

            _context.Usuario.Update(usuarioBuscado);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverUsuario(Usuario usuario)
        {
            usuario.StatusUsuario = false;

            _context.Usuario.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Primitives;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHireAPI.Applications.Conversions;
using NextHireAPI.Convertions.DomainToDTO;
using NextHireAPI.Domains;
using NextHireAPI.DTOs.UsuarioDto;
using NextHireAPI.Interfaces;
using NextHireAPI.Repository;

namespace NextHireAPI.Applications.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly RedisService _redisService;

        public UsuarioService(IUsuarioRepository repository, RedisService redisService)
        {
            _repository = repository;
            _redisService = redisService;
        }

        public async Task<List<ListarUsuariosDto>> ListarUsuarios()
        {
            List<Usuario> candidatos = await _repository.ListarUsuarios()
                ?? throw new DomainException("Nenhum funcionario localizado.");

            return candidatos
                .Select(e => UsuarioToDto.UsuariosToDto(e))
                .ToList();
        }

        public async Task<List<ListarFuncionarioDto>> ListarFuncionariosPorEmpresa(int empresaId)
        {
            List<Usuario> funcionarios = await _repository.ListarFuncionariosPorEmpresaId(empresaId)
                ?? throw new DomainException("Nenhum funcionario localizado.");

            return funcionarios
                .Select(f => UsuarioToDto.FuncionarioToDto(f))
                .ToList();
        }

        public async Task<List<ListarCandidatoDto>> ListarCandidatos()
        {
            List<Usuario> candidatos = await _repository.ListarCandidatos()
                ?? throw new DomainException("Nenhum funcionario localizado.");

            return candidatos
                .Select(e => UsuarioToDto.CandidatoToDto(e))
                .ToList();
        }

        public async Task<ListarFuncionarioDto> BuscarFuncionarioPorId(int funcionarioId)
        {
            var funcionario = UsuarioToDto.FuncionarioToDto(await _repository.BuscarFuncionarioPorId(funcionarioId))
                ?? throw new DomainException("Nenhum funcionario localizado.");

            return funcionario;
        }

        public async Task<ListarCandidatoDto> BuscarCandidatoPorId(int candidatoId)
            => UsuarioToDto.CandidatoToDto
            (
                await _repository.BuscarCandidatoPorId(candidatoId)
                    ?? throw new DomainException("Nenhum candidato com este id localizado.")
            );

        public async Task AdicionarFuncionarios(int empresaId, List<AdicionarFuncionarioDto> funcionarios)
        {
            if (empresaId <= 0)
                throw new DomainException("informe um ID de empresa valido.");

            List<Usuario> funcionariosAdicionados =
                funcionarios.Select(f => UsuarioToDto.FuncionarioDtoToDomain(f)).ToList();

            await _repository.AdicionarFuncionarios(empresaId, funcionariosAdicionados);
        }

        public async Task CadastroCandidato(CadastrarCandidatoDto candidato)
        {
            if (EmailService.ValidarEmail(candidato.Nome))
                throw new DomainException("O nome é um campo obrigatório!");

            if (String.IsNullOrEmpty(candidato.Email) || !EmailService.ValidarEmail(candidato.Email))
                throw new DomainException("Digite um email valido.");

            await _repository.CadastrarCandidato(UsuarioToDto.CandidatoDtoToDomain(candidato));
        }

        public async Task AtualizarUsuarioGeral(int usuarioId, AtualizarUsuarioDto usuarioAtualizado)
        {
            if (String.IsNullOrWhiteSpace(usuarioAtualizado.Nome) && string.IsNullOrWhiteSpace(usuarioAtualizado.Email))
                throw new DomainException("Altere pelo menos um campo!");

            if (!string.IsNullOrWhiteSpace(usuarioAtualizado.Email) && !EmailService.ValidarEmail(usuarioAtualizado.Email))
                throw new DomainException("Digite um email válido.");

            await _repository.AtualizarUsuarioGeral(usuarioId, UsuarioToDto.AtualizarUsuarioDtoToDomain(usuarioAtualizado));
        }

        public async Task AtualizarSenha(string email, string senha, string codigoVerificacao)
        {
            Usuario usuario = await _repository.BuscarUsuarioPorEmail(email);

            if (String.IsNullOrWhiteSpace(senha))
                throw new DomainException("A senha é obrigatória!");

            if (!await _redisService.ValidarCodigo(usuario.UsuarioId, codigoVerificacao))
                throw new DomainException("Codigo de verificacao inválido!");

            byte[] salt = PassToArgon2.GenerateSalt();
            string novaSenha = PassToArgon2.HashPassword(senha, salt);

            await _repository.AtualizarSenha(usuario.UsuarioId, novaSenha);
        }

        public async Task RemoverUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new DomainException("Digite um ID de usuario válido.");

            Usuario usuario = await _repository.BuscarCandidatoPorId(usuarioId) ?? await _repository.BuscarFuncionarioPorId(usuarioId);
            if (usuario == null)
                throw new DomainException("Nenhum usuario localizado.");

            await _repository.RemoverUsuario(usuario);
        }

        public async Task EnviarCodigo(string emailDestinatario)
        {
            if (!EmailService.ValidarEmail(emailDestinatario))
                throw new DomainException("Digite um email valido");

            MailAddress emailConvertido = EmailService.ConverteParaMailAdress(emailDestinatario);

            Usuario usuario = await _repository.BuscarUsuarioPorEmail(emailDestinatario)
                ?? throw new DomainException("Usuario não localizado.");

            Console.WriteLine("1 - Antes de enviar email");

            string codigo = EmailService.GerarCodigo();

            Console.WriteLine("1 - Código gerado");

            try
            {
                await EmailService.EnviarCodigoVerificacao(emailConvertido, codigo);

                Console.WriteLine("2 - Email enviado");
                Console.WriteLine("3 - Chamando RedisService");

                await _redisService.SalvarCodigo(usuario.UsuarioId, codigo);

                Console.WriteLine("4 - Código salvo no Redis");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex}");

                throw new DomainException(ex.Message);
            }
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration.UserSecrets;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHireAPI.Applications.Conversions;
using NextHireAPI.Applications.Services;
using NextHireAPI.Domains;
using NextHireAPI.DTOs.UsuarioDto;

namespace NextHireAPI.Convertions.DomainToDTO
{
    public static class UsuarioToDto
    {
        public static ListarUsuariosDto UsuariosToDto(Usuario usuario)
        {
            var empresa = usuario.Empresa?.FirstOrDefault();

            return new ListarUsuariosDto
            {
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                PrimeiroAcesso = usuario.PrimeiroAcesso,
                StatusUsuario = usuario.StatusUsuario,
                TipoUsuario = usuario.TipoUsuario
            };
        }

        public static ListarFuncionarioDto FuncionarioToDto(Usuario usuario)
        {
            var empresa = usuario.Empresa?.FirstOrDefault();

            return new ListarFuncionarioDto
            {
                FuncionarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                EmpresaId = empresa?.EmpresaId ?? throw new DomainException("Funcionario não locallizado!"),
                NomeEmpresa = empresa?.Nome ?? throw new DomainException("Funcionario não locallizado!"),
                PrimeiroAcesso = usuario.PrimeiroAcesso,
                StatusUsuario = usuario.StatusUsuario,
                TipoUsuario = usuario.TipoUsuario
            };
        }

        public static ListarCandidatoDto CandidatoToDto(Usuario usuario)
        {
            return new ListarCandidatoDto
            {
                CandidatoId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                PrimeiroAcesso = usuario.PrimeiroAcesso,
                StatusUsuario = usuario.StatusUsuario,
                TipoUsuario = usuario.TipoUsuario,
                CurriculoCandidato = usuario.CurriculoCandidato,
                ExperienciaProfissional = usuario.ExperienciaProfissional,
                HabilidadeCandidato = usuario.HabilidadeCandidato,
                InformacaoCandidato = usuario.InformacaoCandidato
            };
        }

        public static AtualizarUsuarioDto AtualizarUsuarioDto(Usuario usuario)
        {
            return new AtualizarUsuarioDto
            {
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }

        public static Usuario FuncionarioDtoToDomain(AdicionarFuncionarioDto funcionario)
        {
            string[] nomeSpt = funcionario.Nome.Split(' ');
            byte[] salt = PassToArgon2.GenerateSalt();
            string novaSenha = PassToArgon2.HashPassword($"{nomeSpt[0]}@2026", salt);

            return new Usuario
            {
                Nome = funcionario.Nome ?? throw new DomainException("O nome do funcionário é obrigatório!"),
                Email = EmailService.ValidarEmail(funcionario.Email) == true ? funcionario.Email : throw new DomainException("Digite um email valido!"),
                Senha = novaSenha,
                TipoUsuario = funcionario.Cargo == TipoUsuarioEnum.AnalistaRh ? TipoUsuarioEnum.AnalistaRh : TipoUsuarioEnum.Gestor
            };
        }

        public static Usuario CandidatoDtoToDomain(CadastrarCandidatoDto candidato)
        {
            byte[] salt = PassToArgon2.GenerateSalt();
            string novaSenha = PassToArgon2.HashPassword(candidato.senha, salt);

            return new Usuario
            {
                Nome = candidato.Nome,
                Email = candidato.Email,
                Senha = novaSenha,
                TipoUsuario = TipoUsuarioEnum.Candidato,
                StatusUsuario = true
            };
        }

        public static Usuario AtualizarUsuarioDtoToDomain(AtualizarUsuarioDto usuario)
        {
            return new Usuario
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
            };
        }
    }
}
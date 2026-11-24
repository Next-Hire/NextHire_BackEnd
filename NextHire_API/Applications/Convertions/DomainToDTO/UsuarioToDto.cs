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
            //? Limpa as referências de volta (candidato) das coleções aninhadas
            //? para evitar ciclos de objeto na serialização JSON (erro 500)
            foreach (var curriculo in usuario.CurriculoCandidato)
            {
                curriculo.Candidato = null;

                foreach (var candidatura in curriculo.CurriculoVaga)
                    candidatura.Curriculo = null;
            }

            foreach (var experiencia in usuario.ExperienciaProfissional)
                experiencia.Usuario = null;

            foreach (var habilidade in usuario.HabilidadeCandidato)
            {
                habilidade.Candidato = null;

                //? a navegação Habilidade agora é incluída na busca; limpa os
                //? retornos para não reintroduzir ciclos na serialização
                if (habilidade.Habilidade != null)
                {
                    habilidade.Habilidade.HabilidadeCandidato.Clear();
                    habilidade.Habilidade.Vaga.Clear();
                }
            }

            foreach (var info in usuario.InformacaoCandidato)
                info.Candidato = null;

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
            //? Listagem de habilidades do candidato ordenada do maior nível para o menor
            HabilidadeCandidato = usuario.HabilidadeCandidato
                .OrderByDescending(hc => hc.Nivel)
                .ToList(),
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
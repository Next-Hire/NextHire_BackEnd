using NextHireAPI.Domains;
using NextHireAPI.DTOs.Autenticacao;
using NextHireAPI.Interfaces;
using NextHireAPI.Applications.Conversions;
using NextHireAPI.Applications.Auth;
using NextHire_API.Domains;
using NextHire_API.Exceptions;

namespace NextHireAPI.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _repository;
        private readonly TokenJWT _jwt;

        public AutenticacaoService(IUsuarioRepository repository, TokenJWT jwt)
        {
            _repository = repository;
            _jwt = jwt;
        }

        public async Task<TokenResponseDto> Login(string email, string senha)
        {
            Usuario usuario = await _repository.BuscarUsuarioPorEmail(email);
            if (usuario == null)
                throw new DomainException("Erro! email ou senha inválidos!");

            if (!PassToArgon2.VerifyPassword(senha, usuario.Senha))
                throw new DomainException("Erro! email ou senha inválidos!");

            string tokenJwt = _jwt.GerarToken(usuario);

            TokenResponseDto tokenDto = new TokenResponseDto
            {
                Token = tokenJwt,
                TipoUsuario = usuario.TipoUsuario.ToString()
            };

            return tokenDto;
        }
    }
}
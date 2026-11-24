using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHireAPI.Applications.Services;
using NextHireAPI.DTOs.Autenticacao;

namespace NextHire.Controllers
{
    /// <summary>Endpoints de autenticação de usuários, responsáveis pela emissão do token JWT.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly AutenticacaoService _service;
        public AutenticacaoController(AutenticacaoService service)
            => _service = service;

        /// <summary>Autentica o usuário com e-mail e senha e retorna um token JWT.</summary>
        /// <param name="loginDto">Credenciais de acesso: e-mail e senha.</param>
        /// <returns>Token JWT com os dados de autenticação do usuário.</returns>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Autentica o usuário", Description = "Valida e-mail e senha e, em caso de sucesso, retorna um token JWT contendo o perfil do usuário. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(200, "Autenticação realizada com sucesso.", typeof(TokenResponseDto))]
        [SwaggerResponse(404, "Credenciais inválidas ou usuário não encontrado.")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                return Ok(await _service.Login(loginDto.email, loginDto.senha));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

            }
        }
    }
}
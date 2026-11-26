using Amazon.Runtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Auth;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHireAPI.Applications.Services;
using NextHireAPI.DTOs.Autenticacao;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http.Headers;
using System.Text.Json;

namespace NextHire.Controllers
{
    /// <summary>Endpoints de autenticação de usuários, responsáveis pela emissão do token JWT.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly AutenticacaoService _service;
        
        public AutenticacaoController(AutenticacaoService service)
            { 
            _service = service;

            }

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


        [HttpGet("linkedin")]
        [AllowAnonymous]
        public IActionResult LinkedInAuth()
        {
            var clientId = Environment.GetEnvironmentVariable("LINKEDIN_CLIENT_ID");
            var redirectUri = Uri.EscapeDataString(Environment.GetEnvironmentVariable("LINKEDIN_URL_BACK")!);
            var state = Guid.NewGuid().ToString("N");
            var scope = Uri.EscapeDataString("openid profile email");

            var authUrl = $"https://www.linkedin.com/oauth/v2/authorization" +
                          $"?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&state={state}&scope={scope}";

            return Redirect(authUrl);
        }

        // 2. Recebe o retorno do LinkedIn, valida/cria o usuário e gera o SEU TokenJWT
        [HttpGet("linkedin/callback")]
        [AllowAnonymous]
        public async Task<IActionResult> LinkedInCallback([FromQuery] string code, [FromQuery] string? error)
        {
            try
            {
                var e = await _service.LinkedInLogin(code, error);
                return Redirect(e);
            }

            catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
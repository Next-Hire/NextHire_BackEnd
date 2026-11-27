using Amazon.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace NextHire_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _client;

        public AuthController(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _client = httpClientFactory;
        }


        [HttpGet("LinkedIn")]
        public ActionResult LinkedInAuth()
        {
            var clientId = Environment.GetEnvironmentVariable("LINKEDIN_CLIENT_ID");
            var redirectUri = Uri.EscapeDataString(Environment.GetEnvironmentVariable("LINKEDIN_URL_BACK"));
            var state = Guid.NewGuid().ToString("N"); // Em produção, salve em cache/cookie para validar no callback
            var scope = Uri.EscapeDataString("openid profile email");

            var authUrl = $"https://www.linkedin.com/oauth/v2/authorization" +
                          $"?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&state={state}&scope={scope}";

            return Redirect(authUrl);
        }

        [HttpGet("linkedin/callback")]
        public async Task<IActionResult> LinkedInInCallback([FromQuery] string code, [FromQuery] string? error)
        {
            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
                return BadRequest("Falha ou cancelamento na autenticação com o LinkedIn.");

            var client = _client.CreateClient();

            // Troca o 'code' pelo 'access_token'
            var tokenRequest = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["client_id"] = Environment.GetEnvironmentVariable("LINKEDIN_CLIENT_ID")!,
                ["client_secret"] = Environment.GetEnvironmentVariable("LINKEDIN_CLIENT_PRIMARY_SECRET")!,
                ["redirect_uri"] = Environment.GetEnvironmentVariable("LINKEDIN_URL_BACK")!
            });

            var tokenResponse = await client.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", tokenRequest);
            if (!tokenResponse.IsSuccessStatusCode)
                return BadRequest("Erro ao obter access token do LinkedIn.");

            var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
            var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

            // Busca os dados do perfil via OpenID Connect (/v2/userinfo)
            var userRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.linkedin.com/v2/userinfo");
            userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var userResponse = await client.SendAsync(userRequest);
            if (!userResponse.IsSuccessStatusCode)
                return BadRequest("Erro ao buscar dados do usuário no LinkedIn.");

            var userJson = JsonDocument.Parse(await userResponse.Content.ReadAsStringAsync()).RootElement;
            var linkedInId = userJson.GetProperty("sub").GetString();
            var email = userJson.GetProperty("email").GetString();
            var name = userJson.GetProperty("name").GetString();
            var picture = userJson.TryGetProperty("picture", out var pic) ? pic.GetString() : null;

            // TODO: Buscar ou cadastrar o usuário no seu banco de dados usando 'email' ou 'linkedInId'
            // TODO: Gerar o JWT da sua própria aplicação
            var seuJwtToken = "GERAR_SEU_JWT_AQUI";

            // Devolve o usuário para o Next.js com o seu JWT
            var frontendUrl = Environment.GetEnvironmentVariable("LINKEDIN_URL_FRONT");
            return Redirect($"{frontendUrl}?token={seuJwtToken}");
        }


    }
}

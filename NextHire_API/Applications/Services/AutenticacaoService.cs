using Microsoft.AspNetCore.Http.HttpResults;
using NextHire_API.Applications.Auth;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHireAPI.Applications.Conversions;
using NextHireAPI.Domains;
using NextHireAPI.DTOs.Autenticacao;
using NextHireAPI.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NextHireAPI.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _repository;
        private readonly TokenJWT _jwt;
        private readonly IHttpClientFactory _httpClient;

        public AutenticacaoService(IUsuarioRepository repository, TokenJWT jwt, IHttpClientFactory httpClient)
        {
            _repository = repository;
            _jwt = jwt;
            _httpClient = httpClient;


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


        private async Task<string> salvarUsuarioOAuth(string email, string nome)
        {
            Usuario? usuarioExiste = await _repository.BuscarUsuarioPorEmail(email);
            if (usuarioExiste == null)
            {
                Usuario usuarioQuery = new Usuario()
                {
                    Nome = nome,
                    Email = email,
                    Senha = "",
                    is_OAuth = true
                };

                await _repository.CadastrarCandidato(usuarioQuery);
                usuarioExiste = await _repository.BuscarUsuarioPorEmail(email);
            }


            var tokenJWT = _jwt.GerarToken(usuarioExiste);
            var frontEndURL = Environment.GetEnvironmentVariable("LINKEDIN_URL_FRONT");

            return $"{frontEndURL}?=token{tokenJWT}";





        }
        public async Task<string> LinkedInLogin(string code, string erro)
        {
            if (!string.IsNullOrEmpty(erro) || string.IsNullOrEmpty(code))
                throw new DomainException("Falha ou cancelamento na autenticação com o LinkedIn");

            var client = _httpClient.CreateClient();

            FormUrlEncodedContent? tokenRequest = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["client_id"] = Environment.GetEnvironmentVariable("LINKEDIN_CLIENT_ID")!,
                ["client_secret"] = Environment.GetEnvironmentVariable("LINKEDIN_CLIENT_PRIMARY_SECRET")!,
                ["redirect_uri"] = Environment.GetEnvironmentVariable("LINKEDIN_URL_BACK")!
            });

            var tokenResponse = await client.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", tokenRequest);
            if (!tokenResponse.IsSuccessStatusCode)
                throw new DomainException("Erro ao obter acess token do LinkedIn");

            var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
            var acessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

            var userRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.linkedin.com/v2/userinfo");
            userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", acessToken);

            var userResponse = await client.SendAsync(userRequest);
            if (!userResponse.IsSuccessStatusCode)
                throw new DomainException("Erro ao buscar dados do usuário no LinkedIn");

            var usuarioJson = JsonDocument.Parse(await userResponse.Content.ReadAsStringAsync()).RootElement;
            var email = usuarioJson.GetProperty("email").GetString()!.Trim().ToLower();
            var nome = usuarioJson.GetProperty("name").GetString() ?? "UsuárioLinkedin";


            return await salvarUsuarioOAuth(email, nome);







        }  

        public async Task<string> GoogleLinkLogin(string code, string? error)
        {
            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
                throw new DomainException("Falha ou cancelamento na autenticação com o Google");

            var client = _httpClient.CreateClient();

            // 1. Troca o 'code' pelo 'access_token' no endpoint do Google
            var tokenRequest = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["client_id"] = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")!,
                ["client_secret"] = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET")!,
                ["redirect_uri"] = Environment.GetEnvironmentVariable("GOOGLE_URL_BACK")!
            });

            var tokenResponse = await client.PostAsync("https://oauth2.googleapis.com/token", tokenRequest);
            if (!tokenResponse.IsSuccessStatusCode)
                throw new DomainException("Erro ao obter access token do Google");

            JsonDocument? tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
            string? accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

            //  Busca os dados do perfil via OpenID Connect do Google
            HttpRequestMessage? userRequest = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v3/userinfo");
            userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var userResponse = await client.SendAsync(userRequest);
            if (!userResponse.IsSuccessStatusCode)
                throw new DomainException("Erro ao buscar dados do usuário no Google");

            var usuarioJson = JsonDocument.Parse(await userResponse.Content.ReadAsStringAsync()).RootElement;
            var email = usuarioJson.GetProperty("email").GetString()!.Trim().ToLower();
            var nome = usuarioJson.GetProperty("name").GetString() ?? "Usuário Google";


            return await salvarUsuarioOAuth(email, nome);
        }
    }
}
using NextHire_API.DTOs.HabilidadeDto;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace NextHire_API.Applications.Integrations
{
    public class RbosfHabilidadeClient : IRbosfHabilidadeClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;

        public RbosfHabilidadeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<RbosfHabilidadeDto>> BuscarPorCodigoAsync(string codigo, CancellationToken cancellationToken = default)
            => BuscarAsync($"habilidade/codigo/{Uri.EscapeDataString(codigo)}", cancellationToken);

        public Task<List<RbosfHabilidadeDto>> BuscarPorTermoAsync(string termo, string? classificacao = null, CancellationToken cancellationToken = default)
        {
            string caminho = $"habilidade/termo/{Uri.EscapeDataString(termo)}";

            if (!string.IsNullOrWhiteSpace(classificacao))
                caminho += $"?classificacao={Uri.EscapeDataString(classificacao)}";

            return BuscarAsync(caminho, cancellationToken);
        }

        private async Task<List<RbosfHabilidadeDto>> BuscarAsync(string caminho, CancellationToken cancellationToken)
        {
            using HttpResponseMessage resposta = await _httpClient.GetAsync(caminho, cancellationToken);

            if (resposta.StatusCode == HttpStatusCode.NotFound)
                return new List<RbosfHabilidadeDto>();

            if (!resposta.IsSuccessStatusCode)
            {
                string corpo = await resposta.Content.ReadAsStringAsync(cancellationToken);

                if (corpo.Length > 300)
                    corpo = corpo[..300];

                throw new DomainException($"A API RBOSF retornou {(int)resposta.StatusCode}: {corpo}");
            }

            List<RbosfHabilidadeDto>? habilidades =
                await resposta.Content.ReadFromJsonAsync<List<RbosfHabilidadeDto>>(JsonOptions, cancellationToken);

            return habilidades ?? new List<RbosfHabilidadeDto>();
        }
    }
}

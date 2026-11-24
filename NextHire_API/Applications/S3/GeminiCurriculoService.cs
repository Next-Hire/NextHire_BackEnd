using NextHire_API.DTOs.CurriculoAWS;

using NextHire_API.Exceptions;
using System.Text;
using System.Text.Json;

namespace NextHire_API.Applications.S3
{
    public class GeminiCurriculoService
    {

        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GeminiCurriculoService(HttpClient httpClient)
        {
            //_apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            //?? throw new DomainException("GEMINI_API_KEY não configurada."); ;
            _httpClient = httpClient;
        }
        
        private static string DescobrirMimeTypePorURL(string url) // conferindo o tipo de arquivo do link
                                                                // para retornar na exibicao
        {
            var caminhoLimpo = url.Split('?')[0].ToLowerInvariant();
            
            if (caminhoLimpo.EndsWith(".png")) 
                return "image/png";
            
            if (caminhoLimpo.EndsWith(".jpg") || caminhoLimpo.EndsWith(".jpeg")) 
                return "image/jpeg";
            
            return "application/pdf";
        }
        
        private async Task<CurriculoDTO?> ProcessarBytesNoGeminiAsync(byte[] bytesArquivo, string mimeType)
    {
        string base64File = Convert.ToBase64String(bytesArquivo);

        var responseSchema = new
        {
            type = "OBJECT",
            properties = new
            {
                nomeCompleto = new { type = "STRING" },
                email = new { type = "STRING" },
                telefone = new { type = "STRING" },
                resumoProfissional = new { type = "STRING" },
                habilidades = new { type = "ARRAY", items = new { type = "STRING" } },
                experiencias = new
                {
                    type = "ARRAY",
                    items = new
                    {
                        type = "OBJECT",
                        properties = new
                        {
                            cargo = new { type = "STRING" },
                            empresa = new { type = "STRING" },
                            periodo = new { type = "STRING" },
                            descricao = new { type = "STRING" }
                        },
                        required = new[] { "cargo", "empresa" }
                    }
                }
            },
            required = new[] { "nomeCompleto", "experiencias", "habilidades" }
        };

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = "Extraia todas as informações deste currículo de forma precisa." },
                        new
                        {
                            inline_data = new
                            {
                                mime_type = mimeType,
                                data = base64File
                            }
                        }
                    }
                }
            },
            generationConfig = new
            {
                responseMimeType = "application/json",
                responseSchema = responseSchema,
                temperature = 0.1
            }
        };

        // Atenção: verifique o nome exato do modelo (ex: gemini-2.5-flash ou gemini-2.0-flash)
        //var url2 = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.5-flash-lite:generateContent?key={_apiKey}";
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        //Enviar requisição e processar resposta
        HttpResponseMessage? response = await _httpClient.PostAsync(url, content);
        string? responseString = await response.Content.ReadAsStringAsync();

        //caso possua erro
        int statusCodeError = ((int)response.StatusCode);

        if (!response.IsSuccessStatusCode)
            throw new DomainException($"Erro do GoogleGemini ({statusCodeError}): {responseString}");
        using var doc = JsonDocument.Parse(responseString);

        var jsonResult = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        if (string.IsNullOrEmpty(jsonResult))
            throw new DomainException("JSON não gerado pelo Gemini.");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<CurriculoDTO>(jsonResult, options);
    }
        
        public async Task<CurriculoDTO?> ExtrairCurriculoArquivoAsync(IFormFile arquivo)
        {
            using var memoryStream = new MemoryStream();
            await arquivo.CopyToAsync(memoryStream);
            return await ProcessarBytesNoGeminiAsync(memoryStream.ToArray(), arquivo.ContentType);
        }

        public async Task<CurriculoDTO?> ExtrairDadosPorUrlAsync(string urlArquivo)
        {
            if (string.IsNullOrWhiteSpace(urlArquivo))
                throw new DomainException("A URL do currículo é obrigatória.");

            // Baixa o arquivo do link da AWS para a memória
            using var response = await _httpClient.GetAsync(urlArquivo);

            if (!response.IsSuccessStatusCode)
                throw new DomainException($"Não foi possível baixar o arquivo da AWS (Status: {(int)response.StatusCode})." +
                                          $" Verifique se o link não expirou.");

            byte[] bytesArquivo = await response.Content.ReadAsByteArrayAsync();

            // Detecta automaticamente o MIME type enviado pelo S3 se eh pdf ou img (ex: "application/pdf", "image/png")
            string mimeType = response.Content.Headers.ContentType?.MediaType 
                              ?? DescobrirMimeTypePorURL(urlArquivo);

            return await ProcessarBytesNoGeminiAsync(bytesArquivo, mimeType);
        }


    }
}

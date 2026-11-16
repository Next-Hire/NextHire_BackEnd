using NextHire_API.DTOs.CurriculoAWS;
using NextHire_API.DTOs.CurriculoS3DTO;
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
            _apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            ?? throw new Exception("GEMINI_API_KEY não configurada."); ;
            _httpClient = httpClient;
        }

        public async Task<CurriculoDTO?> ExtrairDadosAsync(IFormFile arquivo)
        {
            using var memoryStream = new MemoryStream();
            await arquivo.CopyToAsync(memoryStream);
            string? base64File = Convert.ToBase64String(memoryStream.ToArray());
            

            // 2. Definir o Schema JSON esperado pelo Gemini
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

            // Montando o payload da requisição
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
                                mime_type = arquivo.ContentType, // ex: "application/pdf"
                                data = base64File
                            }
                        }
                    }
                }
            },
                generationConfig = new
                {
                    responseMimeType = "application/json", // Força saída JSON
                    responseSchema = responseSchema,       // Garante a estrutura exata
                    temperature = 0.1                      // Baixa criatividade para evitar alucinações
                }
            };

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.5-flash-lite:generateContent?key={_apiKey}";
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            // 4. Enviar requisição e processar resposta
            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            //caso possua erro
            var statusCodeError = ((int)response.StatusCode);

            if (!response.IsSuccessStatusCode)
                throw new DomainException($"Erro do GoogleGemini ({statusCodeError}): {responseString}");
            using var doc = JsonDocument.Parse(responseString);

            // Extrai o texto JSON gerado dentro da estrutura padrão de resposta do Gemini
            var jsonResult = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            if (string.IsNullOrEmpty(jsonResult))
                throw new DomainException("JSON não gerado"); // retornar null talvez

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<CurriculoDTO>(jsonResult, options);
        }



    }
}

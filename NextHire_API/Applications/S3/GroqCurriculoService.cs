using System.Net.Http.Headers;
using NextHire_API.DTOs.CurriculoAWS;
using NextHire_API.DTOs.CurriculoS3DTO;
using NextHire_API.Exceptions;
using System.Text;
using System.Text.Json;
using UglyToad.PdfPig;

namespace NextHire_API.Applications.S3
{
    public class GroqCurriculoService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private const string GroqEndpoint = "https://api.groq.com/openai/v1/chat/completions";

        // modelos que a conta do groq permite usar
        private const string modeloTextoPdf = "openai/gpt-oss-120b";
        private const string modeloVisaoComputacional = "qwen/qwen3.8-27b";

        public GroqCurriculoService(HttpClient httpClient)
        {
            _apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY")
                ?? throw new DomainException("GROQ_API_KEY não configurada.");
            _httpClient = httpClient;
        }

        private static string DescobrirMimeTypePorURL(string url)
        {
            var caminhoLimpo = url.Split('?')[0].ToLowerInvariant();
            if (caminhoLimpo.EndsWith(".png"))
                return "image/png";
            if (caminhoLimpo.EndsWith(".jpg") || caminhoLimpo.EndsWith(".jpeg"))
                return "image/jpeg";

            return "application/pdf";
        }

        private static string ExtrairTextoDePdf(byte[] bytesPdf)
        {
            using var stream = new MemoryStream(bytesPdf);
            using var pdf = PdfDocument.Open(stream);
            var stringConstrutor = new StringBuilder();

            foreach (var pagina in pdf.GetPages())
                stringConstrutor.AppendLine(pagina.Text);
            return stringConstrutor.ToString();
        }

        private async Task<CurriculoDTO?> ProcessarBytesNaGroqAsync(byte[] bytesArquivo, string mimeType)
        {
            string promptInstrucao = """
                Extraia todas as informações deste currículo de forma precisa e retorne APENAS um objeto JSON válido seguindo exatamente esta estrutura:
                {
                  "nomeCompleto": "string",
                  "email": "string",
                  "telefone": "string",
                  "resumoProfissional": "string",
                  "habilidades": ["string"],
                  "experiencias": [
                    {
                      "cargo": "string",
                      "empresa": "string",
                      "periodo": "string",
                      "descricao": "string"
                    }
                  ]
                }
                """;

            string modeloSelecionado;
            object userContent;

            if (mimeType == "application/pdf")
            {
                string textoCurriculo = ExtrairTextoDePdf(bytesArquivo);

                if (string.IsNullOrWhiteSpace(textoCurriculo))
                    throw new DomainException("Não foi possível extrair texto do PDF (pode ser um PDF escaneado como imagem).");

                modeloSelecionado = modeloTextoPdf;
                userContent = $"{promptInstrucao}\n\nConteúdo do currículo:\n{textoCurriculo}";
            }
            else if (mimeType.StartsWith("image/"))
            {
                string base64Image = Convert.ToBase64String(bytesArquivo);
                modeloSelecionado = modeloVisaoComputacional;

                userContent = new object[]
                {
                    new { type = "text", text = promptInstrucao },
                    new
                    {
                        type = "image_url",
                        image_url = new { url = $"data:{mimeType};base64,{base64Image}" }
                    }
                };
            }
            else
                throw new DomainException($"Formato de arquivo não suportado pela Groq: {mimeType}");

            var requestBody = new
            {
                model = modeloSelecionado,
                messages = new object[]
                {
                    new { role = "system", content = "Você é um especialista em RH que extrai dados estruturados de currículos em JSON." },
                    new { role = "user", content = userContent }
                },
                response_format = new { type = "json_object" },
                temperature = 0.1
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, GroqEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new DomainException($"Erro da Groq ({(int)response.StatusCode}): {responseString}");

            using var doc = JsonDocument.Parse(responseString);

            var jsonResult = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrEmpty(jsonResult))
                throw new DomainException("JSON não gerado pela Groq.");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<CurriculoDTO>(jsonResult, options);
        }

        public async Task<CurriculoDTO?> ExtrairCurriculoArquivoAsync(IFormFile arquivo)
        {
            using var memoryStream = new MemoryStream();
            await arquivo.CopyToAsync(memoryStream);
            return await ProcessarBytesNaGroqAsync(memoryStream.ToArray(), arquivo.ContentType);
        }

        public async Task<CurriculoDTO?> ExtrairDadosPorUrlAsync(string urlArquivo)
        {
            if (string.IsNullOrWhiteSpace(urlArquivo))
                throw new DomainException("A URL do currículo é obrigatória.");

            HttpResponseMessage response = await _httpClient.GetAsync(urlArquivo);

            if (!response.IsSuccessStatusCode)
                throw new DomainException($"Não foi possível baixar o arquivo da AWS \n (Status: {(int)response.StatusCode}). \n  Verifique se o link não expirou.");

            byte[] bytesArquivo = await response.Content.ReadAsByteArrayAsync();

            string mimeType = response.Content.Headers.ContentType?.MediaType
                              ?? DescobrirMimeTypePorURL(urlArquivo);
            return await ProcessarBytesNaGroqAsync(bytesArquivo, mimeType);
        }
    }
}
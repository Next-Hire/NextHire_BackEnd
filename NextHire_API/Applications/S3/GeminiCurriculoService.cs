namespace NextHire_API.Applications.S3
{
    public class GeminiCurriculoService
    {

        private readonly string apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        private readonly HttpClient _httpClient;

        public GeminiCurriculoService(string apiKey, HttpClient httpClient)
        {
            this.apiKey = apiKey;
            _httpClient = httpClient;
        }
    }
}

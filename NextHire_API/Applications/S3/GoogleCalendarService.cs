using NextHire_API.Exceptions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace NextHire_API.Applications.S3
{


    public interface IGoogleCalendarRepository
    {
        public Task<string> gerarURL(string titulo, DateTime inicio, DateTime fim);
    }
    public class GoogleCalendarService : IGoogleCalendarRepository
    {
    private readonly IConfiguration _config;


        public GoogleCalendarService(IConfiguration config) => _config = config;

        public async Task<string> gerarURL(string titulo, DateTime inicio, DateTime fim)
        {
            string credentialPath = _config["GoogleCalendar:CredentialsPath"]
            ?? throw new DomainException("Caminho das credenciais do Google não configurado.");

            // E-mail do usuário do Google Workspace que será personificado (obrigatório para gerar Meet)
            string workspaceUserEmail = _config["GoogleCalendar:WorkspaceUserEmail"]
                ?? throw new DomainException("E-mail do Workspace não configurado.");

            GoogleCredential credential;
            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(CalendarService.Scope.CalendarEvents)
                    .CreateWithUser(workspaceUserEmail); // Domain-Wide Delegation
            }

            var service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "NextHire API"
            });

            var novoEvento = new Event
            {
                Summary = titulo,
                Start = new EventDateTime
                {
                    DateTimeDateTimeOffset = new DateTimeOffset(inicio, TimeSpan.FromHours(-3)),
                    TimeZone = "America/Sao_Paulo"
                },
                End = new EventDateTime
                {
                    DateTimeDateTimeOffset = new DateTimeOffset(fim, TimeSpan.FromHours(-3)),
                    TimeZone = "America/Sao_Paulo"
                },
                ConferenceData = new ConferenceData
                {
                    CreateRequest = new CreateConferenceRequest
                    {
                        RequestId = Guid.NewGuid().ToString(),
                        ConferenceSolutionKey = new ConferenceSolutionKey
                        {
                            Type = "hangoutsMeet"
                        }
                    }
                }
            };

            var request = service.Events.Insert(novoEvento, "primary");
            request.ConferenceDataVersion = 1; // Obrigatório para criar o link do Meet

            Event eventoCriado = await request.ExecuteAsync();

            if (string.IsNullOrEmpty(eventoCriado.HangoutLink))
                throw new DomainException("O Google Calendar criou o evento, mas não retornou um link do Meet.");

            return eventoCriado.HangoutLink;
        }
    }
}

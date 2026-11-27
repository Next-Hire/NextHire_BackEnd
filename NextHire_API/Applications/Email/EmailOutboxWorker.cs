using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NextHire_API.Applications.Email
{
    public class EmailOutboxWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailOutboxWorker> _logger;

        public EmailOutboxWorker(IServiceScopeFactory scopeFactory, ILogger<EmailOutboxWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int intervalo = int.TryParse(Environment.GetEnvironmentVariable("EMAIL_OUTBOX_INTERVAL_SECONDS"), out int valor) ? valor : 30;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = _scopeFactory.CreateScope();
                    IEmailService email = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    await email.ProcessarPendentesAsync(20, stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Erro ao processar outbox de e-mails");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(intervalo), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}

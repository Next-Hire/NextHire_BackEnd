using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NextHire_API.Applications.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailOutboxStore _outbox;
        private readonly ILogger<SmtpEmailService> _logger;
        private readonly string _raizTemplates;
        private readonly string _host;
        private readonly int _porta;
        private readonly bool _ssl;
        private readonly string _usuario;
        private readonly string _senha;
        private readonly string _remetente;
        private readonly string _nomeRemetente;

        public SmtpEmailService(EmailOutboxStore outbox, IHostEnvironment environment, ILogger<SmtpEmailService> logger)
        {
            _outbox = outbox;
            _logger = logger;
            _raizTemplates = Path.Combine(environment.ContentRootPath, "Templates", "Emails");

            _host = Ambiente("SMTP_HOST", "smtp.gmail.com");
            _porta = int.TryParse(Ambiente("SMTP_PORT", "587"), out int porta) ? porta : 587;
            _ssl = !string.Equals(Ambiente("SMTP_SSL", "true"), "false", StringComparison.OrdinalIgnoreCase);
            _usuario = Ambiente("SMTP_USER", Ambiente("MAIL_TO_SMTP", string.Empty));
            _senha = Ambiente("SMTP_PASS", Ambiente("MAIL_CODE_PASS", string.Empty));
            _remetente = Ambiente("SMTP_FROM", _usuario);
            _nomeRemetente = Ambiente("SMTP_FROM_NAME", "NextHire");
        }

        private static string Ambiente(string chave, string padrao)
        {
            string? valor = Environment.GetEnvironmentVariable(chave);
            return string.IsNullOrWhiteSpace(valor) ? padrao : valor;
        }

        public async Task EnviarAsync(EmailMessage mensagem, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(mensagem.Destinatario) || !mensagem.Destinatario.Contains('@'))
            {
                _logger.LogWarning("E-mail descartado: destinatário inválido para o template {Template}", mensagem.Template);
                return;
            }

            string dedupe = string.IsNullOrWhiteSpace(mensagem.DedupeKey)
                ? $"{mensagem.Template}:{mensagem.Destinatario}:{Guid.NewGuid():N}"
                : mensagem.DedupeKey;

            int? id = await _outbox.EnfileirarAsync(mensagem, dedupe, cancellationToken);
            if (id is null)
            {
                _logger.LogInformation("E-mail ignorado (idempotência): {DedupeKey}", dedupe);
                return;
            }

            await TentarEnviarAsync(id.Value, mensagem, 0, cancellationToken);
        }

        private async Task TentarEnviarAsync(int id, EmailMessage mensagem, int tentativas, CancellationToken cancellationToken)
        {
            try
            {
                (string html, string texto) = EmailTemplateRenderer.Render(_raizTemplates, mensagem.Template, mensagem.Dados);

                var mail = new MailMessage
                {
                    From = new MailAddress(_remetente, _nomeRemetente),
                    Subject = mensagem.Assunto,
                    Body = html,
                    IsBodyHtml = true
                };

                if (!string.IsNullOrWhiteSpace(texto))
                    mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(texto, null, "text/plain"));

                string destinatario = mensagem.Destinatario;
                string? nomeDestinatario = mensagem.NomeDestinatario;
                string redirecionamento = Ambiente("EMAIL_DEV_REDIRECT", string.Empty);

                if (!string.IsNullOrWhiteSpace(redirecionamento))
                {
                    nomeDestinatario = $"[TESTE para {mensagem.Destinatario}] {mensagem.NomeDestinatario}".Trim();
                    destinatario = redirecionamento;
                }

                mail.To.Add(new MailAddress(destinatario, nomeDestinatario));

                if (!string.IsNullOrWhiteSpace(mensagem.ResponderPara))
                    mail.ReplyToList.Add(mensagem.ResponderPara);

                using var smtp = new SmtpClient(_host, _porta)
                {
                    EnableSsl = _ssl,
                    Timeout = 15000,
                    Credentials = new NetworkCredential(_usuario, _senha)
                };

                await smtp.SendMailAsync(mail, cancellationToken);
                await _outbox.MarcarEnviadoAsync(id, cancellationToken);
                _logger.LogInformation("E-mail enviado: template={Template} destinatario={Destinatario}", mensagem.Template, mensagem.Destinatario);
            }
            catch (Exception ex)
            {
                int proxima = tentativas + 1;
                int intervalo = Math.Min(60, (int)Math.Pow(2, proxima));
                await _outbox.MarcarFalhaAsync(id, proxima, ex.Message, intervalo, cancellationToken);
                _logger.LogError(ex, "Falha ao enviar e-mail: template={Template} tentativa={Tentativa}", mensagem.Template, proxima);
            }
        }

        public async Task<int> ProcessarPendentesAsync(int limite = 20, CancellationToken cancellationToken = default)
        {
            List<EmailOutboxItem> pendentes = await _outbox.PendentesAsync(limite, cancellationToken);
            int enviados = 0;

            foreach (EmailOutboxItem item in pendentes)
            {
                var mensagem = new EmailMessage
                {
                    Destinatario = item.Destinatario,
                    NomeDestinatario = item.NomeDestinatario,
                    Assunto = item.Assunto,
                    Template = item.Template,
                    Dados = item.Dados()
                };

                await TentarEnviarAsync(item.Id, mensagem, item.Tentativas, cancellationToken);
                enviados++;
            }

            if (enviados > 0)
                _logger.LogInformation("Outbox processada: {Quantidade} e-mail(s) reprocessado(s)", enviados);

            return enviados;
        }
    }
}

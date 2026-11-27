using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore.Storage;
using NextHire_API.Exceptions;
using NextHire_API.Applications.Email;

namespace NextHireAPI.Applications.Services
{
    public class EmailService()
    {
        private static string CodigoGerado { get; set; }
        private static MailAddress emailEmp = ConverteParaMailAdress(Environment.GetEnvironmentVariable("MAIL_TO_SMTP"));
        private static string senhaAplicativo = Environment.GetEnvironmentVariable("MAIL_CODE_PASS").ToString();

        public static string GerarCodigo(int tamanho = 6)
        {
            const string letras = "ABCDEFGHIJKLMNOPQRSTUV";
            var random = new Random();

            CodigoGerado = new string(Enumerable.Repeat(letras, tamanho)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());

            return CodigoGerado;
        }

        public static bool ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return false;
            // Verifica se o nome contém apenas letras e espaços
            foreach (char c in nome)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                    return false;
            }
            return true;
        }

        public static bool ValidarEmail(string email)
        {
                var addr = new MailAddress(email);
                return addr.Address == email.Trim();
        }

        public static void ValidarEmail2(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Insira um e-mail.");

            email = email.Trim();

            try
            {
                var addr = new MailAddress(email);

                // Garante que não há display name e que possui um domínio com ponto (ex: .com)
                if (addr.Address != email || !addr.Host.Contains('.'))
                    throw new DomainException("E-mail inválido.");
            }
            catch
            {
                throw new DomainException("E-mail inválido.");
            }
        }

        public static MailAddress ConverteParaMailAdress(string emailStr)
        {
            MailAddress email;
            try
            {
                email = new MailAddress(emailStr);
                return email;
            }
            catch (System.Exception)
            {
                throw new FormatException($"E-mail inválido: {emailStr}");
            }
        }


        public static async Task EnviarCodigoVerificacao(MailAddress emailDestinatario, string codigo)
        {
            string host = Ambiente("SMTP_HOST", "smtp.gmail.com");
            int porta = int.TryParse(Ambiente("SMTP_PORT", "587"), out int valor) ? valor : 587;
            bool ssl = !string.Equals(Ambiente("SMTP_SSL", "true"), "false", StringComparison.OrdinalIgnoreCase);
            string usuario = Ambiente("SMTP_USER", emailEmp.ToString());
            string senha = Ambiente("SMTP_PASS", senhaAplicativo);
            string remetente = Ambiente("SMTP_FROM", usuario);
            string nomeRemetente = Ambiente("SMTP_FROM_NAME", "NextHire");

            var dados = new Dictionary<string, string> { ["codigo"] = codigo };
            (string html, string texto) = EmailTemplateRenderer.Render(RaizTemplates(), "codigo-verificacao", dados);

            var mail = new MailMessage
            {
                From = new MailAddress(remetente, nomeRemetente),
                Subject = "Seu código de verificação - NextHire",
                Body = html,
                IsBodyHtml = true
            };
            mail.To.Add(emailDestinatario);
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(texto, null, "text/plain"));

            using var smtp = new SmtpClient(host, porta)
            {
                EnableSsl = ssl,
                Timeout = 15000,
                Credentials = new NetworkCredential(usuario, senha)
            };

            await smtp.SendMailAsync(mail);
        }

        private static string Ambiente(string chave, string padrao)
        {
            string? valor = Environment.GetEnvironmentVariable(chave);
            return string.IsNullOrWhiteSpace(valor) ? padrao : valor;
        }

        private static string RaizTemplates()
        {
            string baseDir = Path.Combine(AppContext.BaseDirectory, "Templates", "Emails");
            return Directory.Exists(baseDir)
                ? baseDir
                : Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Emails");
        }
    }
}
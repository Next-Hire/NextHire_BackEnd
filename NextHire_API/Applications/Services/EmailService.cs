using System.Net;
using System.Net.Mail;

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

        public static bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new MailAddress(email);
                // Compara se o endereço normalizado é igual ao original
                // Isso evita que entradas como "  @  " sejam aceitas
                return addr.Address == email.Trim();
            }
            catch
            {
                return false;
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


        public static async Task EnviarCodigo(MailAddress emailDestinatario, string codigo)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    emailEmp.ToString(),
                    senhaAplicativo
    ),
                Timeout = 15000
            };

            var mail = new MailMessage(emailEmp, emailDestinatario)
            {
                Subject = "Código para alterar senha",
                Body = $"Seu código para redefinir a senha é: {codigo}",
                IsBodyHtml = false
            };

            mail.To.Add(emailDestinatario);
            await smtp.SendMailAsync(mail);
        }
    }
}
using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore.Storage;
using NextHire_API.Exceptions;

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
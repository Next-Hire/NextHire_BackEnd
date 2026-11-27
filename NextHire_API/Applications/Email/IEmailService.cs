namespace NextHire_API.Applications.Email
{
    public interface IEmailService
    {
        Task EnviarAsync(EmailMessage mensagem, CancellationToken cancellationToken = default);

        Task<int> ProcessarPendentesAsync(int limite = 20, CancellationToken cancellationToken = default);
    }
}

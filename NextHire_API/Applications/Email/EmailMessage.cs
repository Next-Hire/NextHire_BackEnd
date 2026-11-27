namespace NextHire_API.Applications.Email
{
    public class EmailMessage
    {
        public string Destinatario { get; set; } = null!;
        public string? NomeDestinatario { get; set; }
        public string Assunto { get; set; } = null!;
        public string Template { get; set; } = null!;
        public Dictionary<string, string> Dados { get; set; } = new();
        public string? DedupeKey { get; set; }
        public string? ResponderPara { get; set; }
    }
}

namespace NextHire_API.Domains
{
    public partial class RepostaQuestaoPayload
    {
        // ID da alternativa escolhida (se for multipla escolha)
        public int? AlternativaId { get; set; }

        // texto da resposta (se for uma questao dissertativa)
        public string? TextoResposta { get; set; }

        // data ou hora que o candidato respondeu
        public DateTime DataResposta { get; set; } = DateTime.UtcNow;
    }
}

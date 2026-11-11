namespace NextHire_API.DTOs.CandidatoQuestaoDTO
{
    public class RespostaCandidatoQuestaoDTO
    {
        public int? questaoId { get; set; }
        
        public string? textoQuestao { get; set; } = string.Empty;
        

    }

    public class RespostaQuestaoPayload
    {
        public List<int> alternativaIds { get; set; } = new();
        public string? textoAlternativa { get; set; } = string.Empty;
    }



}

namespace NextHire_API.DTOs.AvaliacaoHabilidadeDTO
{
    public class AdicionarAvaliacaoHabilidadeDto
    {
        public int CandidatoId { get; set; }
        public int EntrevistaId { get; set; }
        public int HabilidadeId { get; set; }
        public int NivelAvaliado { get; set; }
    }
}

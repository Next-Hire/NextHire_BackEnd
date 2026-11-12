namespace NextHire_API.DTOs.AvaliacaoHabilidadeDTO
{
    public class ListarAvaliacaoHabilidadeDto
    {
        public int CandidatoId { get; set; }
        public string  NomeCandidato { get; set; } = string.Empty;
        public string EmailCandidato { get; set; } = string.Empty;

        public int EntrevistaId { get; set; }
        public string NomeEntrevista { get; set; } = string.Empty;
        public DateOnly HorarioInicio { get; set; }
        public DateOnly HorarioFim { get; set; }

        public int ProcessoSeletivoId { get; set; }
        public string TituloProcesso { get; set; } = string.Empty;

        public int VagaId { get; set; }
        public string TituloVaga { get; set; } = string.Empty;

        public int HabilidadeId { get; set; }
        public string NomeHabilidade { get; set; } = string.Empty;
        public int NivelAvaliado { get; set; }
    }
}

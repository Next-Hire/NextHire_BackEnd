using NextHire_API.Domains;

namespace NextHire_API.DTOs.CandidatoVagaDTO
{
    public partial class ListarCurriculoVagaDto
    {
        public string? NomeCandidato { get; set; }

        public int CandidatoId { get; set; }

        public double NotaMediaQuestoes { get; set; }

        public string? NomeVaga { get; set; }

        public double NotaCompatibilidadeVaga { get; set; }

        public int VagaId { get; set; }

        public DateTime? DataHoraInscricao { get; set; }
    }
}

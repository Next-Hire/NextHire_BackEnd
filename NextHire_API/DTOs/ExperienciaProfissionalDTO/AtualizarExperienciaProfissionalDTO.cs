using NextHire_API.Domains;

namespace NextHire_API.DTOs.ExperienciaProfissionalDTO
{
    public class AtualizarExperienciaProfissionalDto
    {
        public DateTime DataEntrada { get; set; }

        public DateTime? DataSaida { get; set; }

        public bool? AtualmenteTrabalha { get; set; }
    }
}

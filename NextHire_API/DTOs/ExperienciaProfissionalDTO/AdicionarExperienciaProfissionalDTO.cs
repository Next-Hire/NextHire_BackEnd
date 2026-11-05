using NextHire_API.Domains;

namespace NextHire_API.DTOs.ExperienciaProfissionalDTO
{
    public class AdicionarExperienciaProfissionalDto
    {
        public bool? StatusExperiencia { get; set; }

        public DateTime DataEntrada { get; set; }

        public DateTime? DataSaida { get; set; }

        public int UsuarioId { get; set; }
    }
}

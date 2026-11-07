using NextHire_API.Domains;

namespace NextHire_API.DTOs.ExperienciaProfissionalDTO
{
    public class ListarExperienciaProfissionalDto
    {
        public int ExperienciaProfissionalId { get; set; }

        public string? NomeEmpresa { get; set; }

        public string? Cargo { get; set; }


        public string? StatusExperiencia { get; set; }

        public DateOnly DataEntrada { get; set; }

        public DateOnly? DataSaida { get; set; }

        public int UsuarioId { get; set; }
    }
}

using NextHire_API.Domains;

namespace NextHire_API.DTOs.ExperienciaProfissionalDTO
{
    public class AtualizarExperienciaProfissionalDto
    {
        public string? NomeEmpresa { get; set; }

        public string? Cargo { get; set; }

        public DateOnly DataEntrada { get; set; }

        public DateOnly? DataSaida { get; set; }

        public bool? AtualmenteTrabalha { get; set; }
    }
}

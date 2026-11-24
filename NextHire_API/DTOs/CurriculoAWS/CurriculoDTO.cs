namespace NextHire_API.DTOs.CurriculoAWS
{
    public class CurriculoDTO
    {

        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string ResumoProfissional { get; set; } = string.Empty;
        public List<string> Habilidades { get; set; } = new();
        public List<EscolaridadeDTO> Escolaridade { get; set; } = new();
        public List<ExperienciaDTO> Experiencias { get; set; } = new();
        public List<CursoDTO> Cursos { get; set; } = new();


    }
}

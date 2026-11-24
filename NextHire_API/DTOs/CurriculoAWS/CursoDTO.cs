namespace NextHire_API.DTOs.CurriculoAWS
{
    public class CursoDTO
    {
        public string nomeCurso { get; set; } = string.Empty;
        public string descricaoCurso { get; set; } = string.Empty;
        public int periodoEmHoras { get; set; }
        public string instituicao { get; set; } = string.Empty;
    }
}

namespace NextHire_API.DTOs.HabilidadeDto
{
    public class ResultadoImportacaoRbosfDto
    {
        public int Solicitados { get; set; }
        public int Importados { get; set; }
        public int Atualizados { get; set; }
        public int SemAlteracao { get; set; }
        public List<ListarHabilidadeDto> Habilidades { get; set; } = new();
        public List<string> NaoEncontrados { get; set; } = new();
    }
}

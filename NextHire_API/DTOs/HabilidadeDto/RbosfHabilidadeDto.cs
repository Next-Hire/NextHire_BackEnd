namespace NextHire_API.DTOs.HabilidadeDto
{
    public class RbosfHabilidadeDto
    {
        public int HabilidadeId { get; set; }
        public string Nome { get; set; } = null!;
        public string? Codigo { get; set; }
        public string? Descricao { get; set; }
        public string? ClassificacaoHabilidade { get; set; }
        public string? TipoConceito { get; set; }
        public List<string>? NomesAlternativos { get; set; }
    }
}

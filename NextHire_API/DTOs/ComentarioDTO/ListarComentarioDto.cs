namespace NextHire_API.DTOs.ComentarioDTO
{
    public class ListarComentarioDto
    {
        public int ComentarioId { get; set; }
        public int EntrevistaId { get; set; }
        public string NomeGestor { get; set; } = null!;
        public int GestorId { get; set; }
        public string Conteudo { get; set; } = null!;
        public DateTime DataComentario { get; set; }
    }
}
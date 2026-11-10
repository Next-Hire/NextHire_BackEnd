namespace NextHire_API.DTOs.ComentarioDTO
{
    public class AdicionarComentarioDto
    {
        public int EntrevistaId { get; set; }
        public int GestorId { get; set; }
        public string Conteudo { get; set; } = null!;
    }
}
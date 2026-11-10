namespace NextHire_API.DTOs.AnotacaoDto
{
    public class ListarAnotacaoDto
    {
        public int AnotacaoId { get; set; }

        public int CandidatoId { get; set; }

        public string Comentario { get; set; } = null!;

        public int GestorId { get; set; }
    }
}

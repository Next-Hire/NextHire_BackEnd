namespace NextHire_API.DTOs.AnotacaoDto
{
    public class AdicionarAnotacaoDto
    {
        public int CandidatoId { get; set; }

        public string Conteudo { get; set; } = null!;

        public int GestorId { get; set; }
    }
}

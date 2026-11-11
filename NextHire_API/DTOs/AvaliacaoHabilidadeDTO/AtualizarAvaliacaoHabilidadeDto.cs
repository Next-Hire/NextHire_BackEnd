namespace NextHire_API.DTOs.AvaliacaoHabilidadeDTO
{
    // Os identificadores fazem parte da chave composta e são informados pela URL.
    // Por isso, somente o nível avaliado pode ser alterado.
    public class AtualizarAvaliacaoHabilidadeDto
    {
        public int NivelAvaliado { get; set; }
    }
}

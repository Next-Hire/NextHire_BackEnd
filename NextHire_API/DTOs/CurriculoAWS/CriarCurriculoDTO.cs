namespace NextHire_API.DTOs.CurriculoAWS;

public class CriarCurriculoDTO
{
    public int candidatoId { get; set; }
    public string nomeCandidato { get; set; } = string.Empty;
    
    public CurriculoDTO curriculoConvertido { get; set; }
}
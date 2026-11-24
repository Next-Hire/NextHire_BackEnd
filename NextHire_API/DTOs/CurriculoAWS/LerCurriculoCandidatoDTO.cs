namespace NextHire_API.DTOs.CurriculoAWS;

public class LerCurriculoCandidatoDTO
{
    public int curriculoCandidatoId { get; set; }
    public UsuarioCurriculoDTO Usuario { get; set; } = new();
    public CurriculoDTO? CurriculoConvertido { get; set; }
}

public class UsuarioCurriculoDTO
{
    public int candidatoId { get; set; }
    public string nome { get; set; } = string.Empty;
}
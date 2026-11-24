namespace NextHire_API.DTOs.CurriculoAWS
{
    public class EscolaridadeDTO
    {
        public string nomeDaEscolaridade { get; set; } = string.Empty;
        public string tipoDeEscolaridade { get; set; } = string.Empty;
        public int periodoAnos { get; set; }
        public string dataInicio { get; set; } = string.Empty;
        public string dataEncerramento { get; set; } = string.Empty;
        public string instituicao { get; set; } = string.Empty;
 
    }
}

namespace NextHire_API.DTOs.CurriculoS3DTO
{
    public class CurriculoDTOS3
    {
        public int Id { get; set; }
        public string NomeArquivo { get; set; }
        public string ContentType { get; set; } // Ex: "application/pdf"
        public byte[] ArquivoPdf { get; set; }  // Aqui ficará o binário do PDF
        //public DateTime DataUpload { get; set; }
    }
}

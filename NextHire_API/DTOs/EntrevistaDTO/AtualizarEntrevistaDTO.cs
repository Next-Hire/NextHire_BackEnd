using System.ComponentModel.DataAnnotations;

namespace NextHire_API.DTOs.EntrevistaDTO
{
    public class AtualizarEntrevistaDTO
    {
        public int entrevistaId { get; set; }
        public string? tituloEntrevista { get; set; }

        public DateTime? dataInicio { get; set; }

        public DateTime? dataEncerra { get; set; }

        public bool? statusEntrevista { get; set; }
    }
    
    public class AtualizarDataEntrevistaDTO
    {
        [Required(ErrorMessage = "A data/hora de início é obrigatória.")]
        public DateTime dataInicio { get; set; }

        [Required(ErrorMessage = "A data/hora de encerramento é obrigatória.")]
        public DateTime dataEncerra { get; set; }
    }
}

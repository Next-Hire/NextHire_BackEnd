using System.ComponentModel.DataAnnotations;

namespace NextHire_API.DTOs.EntrevistaDTO
{
    public class CriarEntrevistaDTO
    {
        [Required(ErrorMessage = "O título da entrevista é obrigatório.")]
        public string tituloEntrevista { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data/hora de início é obrigatória.")]
        public DateTime dataInicio { get; set; }

        [Required(ErrorMessage = "A data/hora de encerramento é obrigatória.")]
        public DateTime dataEncerra { get; set; }

        [Required(ErrorMessage = "O processo seletivo é obrigatório.")]
        public int ProcessoSeletivoId { get; set; }

        // Opcional: caso queira passar os IDs dos candidatos já na criação
        public List<int> CandidatosIds { get; set; } = new List<int>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace NextHire_API.DTOs.EntrevistaDTO
{
    public class CriarEntrevistaDTO
    {
        [Required(ErrorMessage = "O título da entrevista é obrigatório.")]
        public string tituloEntrevista { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data/hora de início é obrigatória.")]
        /// <summary>Início da entrevista (ISO 8601).</summary>
        /// <example>2026-12-02T14:00:00Z</example>
        public DateTime dataInicio { get; set; }

        [Required(ErrorMessage = "A data/hora de encerramento é obrigatória.")]
        /// <summary>Fim da entrevista (ISO 8601).</summary>
        /// <example>2026-12-02T15:00:00Z</example>
        public DateTime dataEncerra { get; set; }

        [Required(ErrorMessage = "O processo seletivo é obrigatório.")]
        public int ProcessoSeletivoId { get; set; }

        /// <summary>Link da videochamada. Se vazio, usa INTERVIEW_LINK do .env.</summary>
        public string? link { get; set; }

        // Opcional: caso queira passar os IDs dos candidatos já na criação
        public List<int> CandidatosIds { get; set; } = new List<int>();
    }
}

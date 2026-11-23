using System.ComponentModel.DataAnnotations;
using NextHire_API.Domains;

namespace NextHire_API.DTOs.CandidatoProcessoSeletivoDTO;

public class CriarCandidatoProcessoDTO
{
    [Required(ErrorMessage = "O ID do processo seletivo é obrigatório.")]
    public int ProcessoSeletivoId { get; set; }

    [Required(ErrorMessage = "O ID do candidato é obrigatório.")]
    public int CandidatoId { get; set; }

    [Required(ErrorMessage = "O status da relação é obrigatório.")]
    public StatusRelacaoEnum StatusRelacao { get; set; } = StatusRelacaoEnum.Considera;
}
using System.ComponentModel.DataAnnotations;
using NextHire_API.Domains;

namespace NextHire_API.DTOs.CandidatoProcessoSeletivoDTO;

public class AtualizarCandidatoProcessoDTO
{
    [Required(ErrorMessage = "O novo status da relação é obrigatório ")]
    public StatusRelacaoEnum statusRelacao { get; set; }
}
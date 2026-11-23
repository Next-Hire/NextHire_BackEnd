using NextHire_API.Domains;

namespace NextHire_API.DTOs.CandidatoProcessoSeletivoDTO;

public class LerCandidatoProcessoDTO
{
    public int processoSeletivoId { get; set; }
    public string nomeProcesso { get; set; } = string.Empty;
    // public bool? statusProcessoSeletivo { get; set; }

    public int candidatoId { get; set; }
    public string nomeCandidato { get; set; } = string.Empty;
    // public string EmailCandidato { get; set; } = string.Empty;

    public StatusRelacaoEnum StatusRelacao { get; set; }
}
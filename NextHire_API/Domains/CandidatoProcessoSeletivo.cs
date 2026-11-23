namespace NextHire_API.Domains;

public class CandidatoProcessoSeletivo
{
    public int ProcessoSeletivoId { get; set; }
    public int CandidatoId { get; set; }
    public StatusRelacaoEnum StatusRelacao { get; set; }

    public virtual Usuario Candidato { get; set; } = null!;
    public virtual ProcessoSeletivo ProcessoSeletivo { get; set; } = null!;
}
using System.Text.Json.Serialization;

namespace NextHire_API.DTOs.AvaliacaoAgenteDTO;

// ============================================================
// Contrato de saída do Next Agent (LM Studio) — evaluation_user
// Schema: {"nota_geral":0,"comentario_respostas":"...","comentario_curriculo":"..."}
// ============================================================

public class AvaliacaoAgenteDto
{
    /// <summary>Nota geral da candidatura (0 a 100).</summary>
    [JsonPropertyName("nota_geral")]
    public int nota_geral { get; set; }

    /// <summary>Comentário do agente sobre as respostas dadas na vaga.</summary>
    [JsonPropertyName("comentario_respostas")]
    public string comentario_respostas { get; set; } = string.Empty;

    /// <summary>Comentário do agente comparando o currículo com a vaga.</summary>
    [JsonPropertyName("comentario_curriculo")]
    public string comentario_curriculo { get; set; } = string.Empty;
}

// ============================================================
// DTO de leitura (persistido no banco)
// ============================================================

public class ListarAvaliacaoAgenteDto
{
    public int avaliacaoId { get; set; }

    public int vagaId { get; set; }

    public int candidatoId { get; set; }

    public string? nomeCandidato { get; set; }

    public int notaGeral { get; set; }

    public string comentarioRespostas { get; set; } = string.Empty;

    public string comentarioCurriculo { get; set; } = string.Empty;

    public DateTime? dataAvaliacao { get; set; }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace NextHire_API.DTOs.QuestaoDTO;

// ============================================================
// DTOs de escrita (Create / Update)
// ============================================================

public class CriarAlternativaDto
{
    public string textoAlternativa { get; set; } = string.Empty;

    public bool alternativaCorreta { get; set; }
}

public class AdicionarQuestaoDto
{
    public string textoQuestao { get; set; } = string.Empty;

    /// <summary>Valores aceitos: comportamental | tecnica | situacional</summary>
    public string classificacaoQuestao { get; set; } = string.Empty;

    /// <summary>Valores aceitos: verdadeiro_falso | unica_escolha | multipla_escolha | dissertativa</summary>
    public string tipoQuestao { get; set; } = string.Empty;

    public List<CriarAlternativaDto> alternativas { get; set; } = new();
}

/// <summary>
/// Atualização parcial: campos nulos não são alterados.
/// Se "alternativas" vier preenchido, as alternativas atuais são substituídas.
/// </summary>
public class AtualizarQuestaoDto
{
    public string? textoQuestao { get; set; }

    /// <summary>Valores aceitos: comportamental | tecnica | situacional</summary>
    public string? classificacaoQuestao { get; set; }

    /// <summary>Valores aceitos: verdadeiro_falso | unica_escolha | multipla_escolha | dissertativa</summary>
    public string? tipoQuestao { get; set; }

    public List<CriarAlternativaDto>? alternativas { get; set; }
}

// ============================================================
// DTOs de leitura
// ============================================================

public class ListarAlternativaDto
{
    public int alternativaId { get; set; }

    public string textoAlternativa { get; set; } = string.Empty;

    public bool alternativaCorreta { get; set; }
}

public class ListarQuestaoDto
{
    public int questaoId { get; set; }

    public int vagaId { get; set; }

    public string? tituloVaga { get; set; }

    public string textoQuestao { get; set; } = string.Empty;

    public string classificacaoQuestao { get; set; } = string.Empty;

    public string tipoQuestao { get; set; } = string.Empty;

    public List<ListarAlternativaDto> alternativas { get; set; } = new();
}

// ============================================================
// DTO de verificação de respostas do candidato
// ============================================================

public class VerificarQuestaoDto
{
    public int questaoId { get; set; }

    public string textoQuestao { get; set; } = string.Empty;

    public string tipoQuestao { get; set; } = string.Empty;

    /// <summary>true = acertou | false = errou | null = dissertativa (sem correção automática)</summary>
    public bool? acertou { get; set; }

    public List<string> respostasCandidato { get; set; } = new();

    public List<string> respostasCorretas { get; set; } = new();
}

// ============================================================
// Contrato de saída do Next Agent (LM Studio)
// Schema: {"questoes":[{"texto_questao":"...","classificacao_questao":"...",
//          "tipo_questao":"...","dificuldade":"...","alternativas":[...]}]}
// ============================================================

public class AgenteQuestoesRespostaDto
{
    [JsonPropertyName("questoes")]
    public List<QuestaoAgenteDto> questoes { get; set; } = new();
}

public class QuestaoAgenteDto
{
    [JsonPropertyName("texto_questao")]
    public string texto_questao { get; set; } = string.Empty;

    [JsonPropertyName("classificacao_questao")]
    public string classificacao_questao { get; set; } = string.Empty;

    [JsonPropertyName("tipo_questao")]
    public string tipo_questao { get; set; } = string.Empty;

    /// <summary>Não é persistida (tabela questao não possui esta coluna no momento).</summary>
    [JsonPropertyName("dificuldade")]
    public string? dificuldade { get; set; }

    [JsonPropertyName("alternativas")]
    public List<AgenteAlternativaDto> alternativas { get; set; } = new();
}

public class AgenteAlternativaDto
{
    [JsonPropertyName("texto_alternativa")]
    public string texto_alternativa { get; set; } = string.Empty;

    /// <summary>O modelo pode omitir o campo quando for false.</summary>
    [JsonPropertyName("alternativa_correta")]
    public bool? alternativa_correta { get; set; }
}

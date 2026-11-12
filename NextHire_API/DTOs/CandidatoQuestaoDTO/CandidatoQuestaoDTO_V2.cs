using System.Text.Json;

namespace NextHire_API.DTOs.CandidatoQuestaoDTO;


/// <summary>
/// fdsafadsfads
/// </summary>

public class RespostaQuestaoPayloadDTO
{
// JsonElement aceita tanto true/false como texto normal tbm
    public JsonElement? resposta { get; set; }
    
    // textos para multipla escolha
    public List<string>? respostas { get; set; }
}

// DTO para post
public class CriarCandidatoQuestaoDTO
{
    public int questaoId { get; set; }
    public int candidatoId { get; set; }
    public RespostaQuestaoPayloadDTO respostaQuestao { get; set; } = null!;
}

// 3. DTO de Leitura Simples (Evitar ler gabarito)
public class LerCandidatoQuestaoDTO
{
    public int questaoId { get; set; }
    public string textoQuestao { get; set; } = string.Empty;
    public string tipoQuestao { get; set; } = string.Empty;
    public int candidatoId { get; set; }
    public string? nomeCandidato { get; set; }
    public RespostaQuestaoPayloadDTO? respostaQuestao { get; set; }
    public List<AlternativaSemGabaritoDTO> alternativas { get; set; } = new();
}

public class AlternativaSemGabaritoDTO
{
    public int alternativaId { get; set; }
    public string textoAlternativa { get; set; } = string.Empty;
}

// 4. DTO de Leitura com Gabarito e Correção (Para RH/Gestor e ListarRespostasComGabarito / ListaDeAcertos)
public class LerCandidatoQuestaoGabaritoDTO
{
    public int questaoId { get; set; }
    public string textoQuestao { get; set; } = string.Empty;
    public string tipoQuestao { get; set; } = string.Empty;
    public int candidatoId { get; set; }
    public RespostaQuestaoPayloadDTO? respostaQuestao { get; set; }
    public bool? acertou { get; set; } // Null se for dissertativa
    public List<AlternativaComGabaritoDTO> alternativas { get; set; } = new();
}

public class AlternativaComGabaritoDTO
{
    public int alternativaId { get; set; }
    public string textoAlternativa { get; set; } = string.Empty;
    public bool alternativaCorreta { get; set; }
}
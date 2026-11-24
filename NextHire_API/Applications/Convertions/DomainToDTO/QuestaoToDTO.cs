using NextHire_API.Domains;
using NextHire_API.DTOs.QuestaoDTO;

namespace NextHire_API.Applications.Convertions.DomainToDTO;

public class QuestaoToDTO
{
    public static ListarQuestaoDto QuestaoParaListarDto(Questao questao)
    {
        return new ListarQuestaoDto
        {
            questaoId = questao.QuestaoId,
            vagaId = questao.VagaId,
            tituloVaga = questao.Vaga?.Titulo,
            textoQuestao = questao.TextoQuestao,
            classificacaoQuestao = questao.ClasssificacaoQuestao.ToString(),
            tipoQuestao = questao.TipoQuestao.ToString(),
            alternativas = questao.Alternativa
                .Select(a => new ListarAlternativaDto
                {
                    alternativaId = a.AlternativaId,
                    textoAlternativa = a.TextoAlternativa,
                    alternativaCorreta = a.AlternativaCorreta
                })
                .ToList()
        };
    }
}

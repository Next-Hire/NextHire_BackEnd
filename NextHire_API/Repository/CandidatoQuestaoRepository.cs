using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoQuestaoDTO;
using NextHire_API.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace NextHire_API.Repository
{
    public class CandidatoQuestaoRepository : ICandidato_QuestaoRepository
    {
        private readonly NextHireContext _ctx;
        public CandidatoQuestaoRepository(NextHireContext context)  => _ctx = context;

        public async Task<List<CandidatoQuestao>> Listar(int candidatoId, int vagaId) => 
            await _ctx.CandidatoQuestao
                .Where(cq => cq.CandidatoId == candidatoId && cq.Questao.VagaId == vagaId)
                .ToListAsync();

        public async Task<List<CandidatoQuestao>> ListaDeAcertos(int candidatoId, int vagaId)
        {
            var candidatoQuery = await _ctx.Usuario.FindAsync(candidatoId) ?? null;
            var vagaQuery = await _ctx.Vaga.FindAsync(vagaId) ?? null;

            return await _ctx.CandidatoQuestao
                .Include(varAux => varAux.Questao)
                    .ThenInclude(varAux => varAux.Alternativa)
                    .Where(varAux => varAux.CandidatoId == candidatoId && varAux.Questao.VagaId == vagaId)
                    .ToListAsync();
        }

        public async Task<List<CandidatoQuestao>> 


        //public async Task<List<RespostaCandidatoQuestaoDTO>> ListaDeAcertos(int candidatoId)
        //{

        //    var query = _ctx.CandidatoQuestao
        //        .Include(varAux => varAux.Questao)
        //            .ThenInclude(varAux => varAux.Alternativa)
        //            .Where(varAux => varAux.CandidatoId == candidatoId);

        //    var listaResposta = await query.ToListAsync();

        //    return listaResposta.Select(cand_qst =>
        //    {
        //        var ehMultiplaEscolha = cand_qst.Questao.TipoQuestao = TipoQuestaoEnum.MultiplaEscolha;

        //        var alternativaCorretaId = cand_qst.Questao.Alternativa
        //        .FirstOrDefault(altCorreta => altCorreta.AlternativaCorreta)?.AlternativaId;

        //        var alternativaEscolhidaId = cand_qst.RepostaQuestao.FirstOrDefault(); // ?.AlternativaId

        //        return new RespostaCandidatoQuestaoDTO
        //        {
        //            QuestaoId = cand_qst.QuestaoId,
        //            tituloQuestao = cand_qst.Questao.TextoQuestao,
        //            tipoQuestaoEnum = cand_qst.Questao.TipoQuestao.ToString(),
        //            AlternativaEscolhidaId = alternativaEscolhidaId,
        //            alternativaEscolhida = cand_qst.RepostaQuestao,

        //            // Se for múltipla escolha, valida se o candidato marcou a alternativa correta
        //            estaCerta = alternativaCorretaId
        //            //&&
        //            //        alternativaEscolhida.HasValue &&
        //            //        alternativaEscolhida.Value == alternativaCorretaId
        //        };
        //    }


        //    ).ToList();


        //}

        //public Task<List<CandidatoQuestao>> ListaDeErros(int candidatoId)
        //{

        //}

        public async Task<CandidatoQuestao?> Excluir(int id)
        {
            var candidatoQuestao = await _ctx.CandidatoQuestao.FindAsync(id);
            if (candidatoQuestao != null)
            {
                _ctx.CandidatoQuestao.Remove(candidatoQuestao);
                await _ctx.SaveChangesAsync();
            }
            return candidatoQuestao;
        }
    }
}

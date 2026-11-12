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


        public async Task<CandidatoQuestao?> BuscarPorId(int questaoId, int candidatoId)
            => await _ctx.CandidatoQuestao
                .Include(varAux => varAux.Questao)
                    .ThenInclude(varAux => varAux.Alternativa)
                .FirstOrDefaultAsync(varAux => varAux.QuestaoId == questaoId 
                                               && varAux.CandidatoId == candidatoId);

        public async Task<bool?> ExisteRespostaCandidato(int questaoId, int candidatoId)
            => await _ctx.CandidatoQuestao
                .AnyAsync(varAux => varAux.QuestaoId == questaoId && varAux.CandidatoId == candidatoId);
        
        
        public async Task<List<CandidatoQuestao?>> ListarPorCandidato(int candidatoId)
            => await _ctx.CandidatoQuestao
                .AsNoTracking()
                .Where(varAux => varAux.CandidatoId == candidatoId)
                .Include(varAux => varAux.Questao)
                .ToListAsync();

        public async Task<List<CandidatoQuestao>> ListarPorQuestao(int questaoId)
            => await _ctx.CandidatoQuestao
                .AsNoTracking()
                .Where(varAux => varAux.QuestaoId == questaoId)
                .Include(varAux => varAux.Candidato)
                .ToListAsync() ?? null;

        public async Task<List<CandidatoQuestao>> ListarCandidatoEVaga(int candidatoId, int vagaId)
            => await _ctx.CandidatoQuestao
                .AsNoTracking()
                .Where(varAux => varAux.CandidatoId == candidatoId &&
                                 varAux.Questao.VagaId == vagaId)
                .Include(varAux => varAux.Questao)
                .ToListAsync();

        public async Task<List<Questao?>> ListarPorQuestoesEmBranco(int candidatoId, int vagaId) =>
            await _ctx.Questao
                .Where(varAux => varAux.VagaId == vagaId &&
                                 !varAux.CandidatoQuestao.Any(varAux => varAux.CandidatoId == candidatoId))
                .ToListAsync();
        
        



        public async Task<List<CandidatoQuestao>> ListarRepostasCertas(int candidatoId) =>
            await _ctx.CandidatoQuestao
                .AsNoTracking()
                .Where(candQues => candQues.CandidatoId == candidatoId)
                .Include(varAux => varAux.Questao)
                    .ThenInclude(varAux => varAux.Alternativa)
                .ToListAsync();

        public async Task<List<CandidatoQuestao?>> ListarRespostasComGabarito(int candidatoId, int? vagaId = null)
        {
            IQueryable<CandidatoQuestao?> candQuestaoQuery =  _ctx.CandidatoQuestao
                .AsNoTracking()
                .Where(varAux => varAux.CandidatoId == candidatoId);

            if (vagaId.HasValue) // caso haja id da vaga para atrelar à query
                candQuestaoQuery = candQuestaoQuery.Where(varAux => varAux.Questao.VagaId == vagaId);
            
            // QuestaoQuery inteira e + alternativa dela.
            return await candQuestaoQuery
                .Include(varAux => varAux.Questao)
                    .ThenInclude(varAux => varAux.Alternativa) // tentei puxar pelas alternativas certas somente mas não foi
                .ToListAsync(); 

        }
        
        
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

                public async Task<CandidatoQuestao?> Adicionar(CandidatoQuestao candQuestao)
                {
                    await _ctx.CandidatoQuestao.AddAsync(candQuestao);
                    await _ctx.SaveChangesAsync();
                    return candQuestao;
                }
                
                
                public async Task<CandidatoQuestao?> Excluir(int candidatoId, int questaoId)
                {
                    var candidatoQuestao = await _ctx.CandidatoQuestao.FindAsync(questaoId, candidatoId);
                    if (candidatoQuestao != null)
                    {
                        _ctx.CandidatoQuestao.Remove(candidatoQuestao);
                        await _ctx.SaveChangesAsync();
                    }
                    return candidatoQuestao;
                }
                
                
                
    }
}

        // public async Task<List<CandidatoQuestao>> 


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


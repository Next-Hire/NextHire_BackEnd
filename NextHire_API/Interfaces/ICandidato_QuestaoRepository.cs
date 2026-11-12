using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface ICandidato_QuestaoRepository
    {
        public Task<bool?> ExisteRespostaCandidato(int questaoId, int candidatoId);
        public Task<List<CandidatoQuestao?>> ListarPorCandidato(int candidatoId);
        public Task<CandidatoQuestao?> BuscarPorId(int questaoId, int candidatoId);
        
        public Task<List<CandidatoQuestao?>> ListarRepostasCertas(int candidatoId); // Feito
        public Task<List<CandidatoQuestao?>> ListarPorQuestao(int questaoId);
        
        public Task<List<CandidatoQuestao?>> ListarCandidatoEVaga(int candidatoId, int vagaId);
        public Task<List<Questao?>> ListarPorQuestoesEmBranco(int candidatoId, int vagaId);

        public Task<CandidatoQuestao?> Adicionar(CandidatoQuestao candQuestao);
        // public Task<CandidatoQuestao> Atualizar(CandidatoQuestao candQuestao);
        public Task<CandidatoQuestao?> Excluir(int candidatoid, int questaoId);
    }
}

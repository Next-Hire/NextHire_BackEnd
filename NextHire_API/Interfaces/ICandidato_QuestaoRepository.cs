using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface ICandidato_QuestaoRepository
    {
        public Task<List<CandidatoQuestao>> Listar(int candidatoId);

        //public Task<List<CandidatoQuestao>> ListaDeAcertos(int candidatoId);

        //public Task<List<CandidatoQuestao>> ListaDeErros(int candidatoId);



        public Task<CandidatoQuestao?> Excluir(int id);
    }
}

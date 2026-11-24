using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IQuestaoRepository
    {
        public Task<List<Questao>> ListarPorVaga(int vagaId);

        public Task<Questao> BuscarPorId(int questaoId);

        /// <summary>Busca a vaga com os dados necessários para o payload do Next Agent.</summary>
        public Task<Vaga> BuscarVagaParaAgente(int vagaId);

        /// <summary>Questões já existentes da vaga (usadas como contexto no generate_more).</summary>
        public Task<List<Questao>> ListarQuestoesExistentesPorVaga(int vagaId);

        public Task Adicionar(Questao questao);

        public Task AdicionarVarias(List<Questao> questoes);

        public Task Atualizar(int questaoId, Questao dados, List<Alternativa>? alternativas);

        public Task Remover(int questaoId);
    }
}

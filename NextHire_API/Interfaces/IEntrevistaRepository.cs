using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IEntrevistaRepository
    {
        public Task<List<Entrevista>> Listar();
        public Task<List<Entrevista>> ListarPorStatus(bool status);
        public Task<List<Entrevista>> EntrevistasHoje(DateTime data);
        public Task<List<Entrevista>> BuscarPorCandidato(int candidatoId);
        public Task<Entrevista?> BuscarPorId(int id);

        public Task<List<Entrevista>> ListarPorTipo(string tipoEntrevista);
        public Task<Entrevista?> BuscarPorTipo(string tipoEntrevista);

        public Task<Entrevista> AtualizarCandidatoNaEntrevista(int entrevistaId, int candidatoId);
        public Task<Entrevista> AtualizarDataDaEntrevista(int entrevistaId, DateTime? dataInicio, DateTime? dataFinal);
        public Task<Entrevista> AdicionarEntrevista(Entrevista entr, List<int> candidatosIds);
        public Task<Entrevista> RemoverEntrevista(int id);
    }
}

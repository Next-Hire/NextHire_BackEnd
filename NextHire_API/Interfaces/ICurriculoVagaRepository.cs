using NextHire_API.Domains;
using System.Reflection.Metadata.Ecma335;

namespace NextHire_API.Interfaces
{
    public interface ICurriculoVagaRepository
    {
        public Task<List<CurriculoVaga>> ListarCandidatosPorVagaId(int vagaId);

        public Task<List<Vaga>> ListarVagasPorCandidatoId(int curriculoId);
        
        public Task CadastrarCurriculoVaga(int curriculoId, int vagaId);

        public Task Remover(int curriculoId, int vagaId);
    }
}

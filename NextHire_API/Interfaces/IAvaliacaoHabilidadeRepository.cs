using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IAvaliacaoHabilidadeRepository
    {
        Task<List<AvaliacaoHabilidade>> Listar();
        Task<AvaliacaoHabilidade?> BuscarPorId(int candidatoId, int entrevistaId, int habilidadeId);
        Task<List<AvaliacaoHabilidade>> BuscarPorCandidato(int candidatoId);
        Task Adicionar(AvaliacaoHabilidade avaliacaoHabilidade);
        Task<AvaliacaoHabilidade?> Atualizar(int candidatoId, int entrevistaId, int habilidadeId, int nivelAvaliado);
        Task Deletar(int candidatoId, int entrevistaId, int habilidadeId);
    }
}

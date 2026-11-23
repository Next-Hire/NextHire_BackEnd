using NextHire_API.Domains;

namespace NextHire_API.Interfaces;

public interface IAvaliacaoAgenteRepository
{
    /// <summary>
    /// Persiste a avaliação do agente para a candidatura. Se já existir avaliação
    /// para o mesmo candidato e vaga, atualiza (upsert).
    /// </summary>
    public Task<AvaliacaoAgente> Salvar(AvaliacaoAgente avaliacao);

    public Task<AvaliacaoAgente?> BuscarPorCandidatoEVaga(int candidatoId, int vagaId);

    public Task<List<AvaliacaoAgente>> ListarPorVaga(int vagaId);
}

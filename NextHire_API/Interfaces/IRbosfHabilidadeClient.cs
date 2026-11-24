using NextHire_API.DTOs.HabilidadeDto;

namespace NextHire_API.Interfaces
{
    public interface IRbosfHabilidadeClient
    {
        Task<List<RbosfHabilidadeDto>> BuscarPorCodigoAsync(string codigo, CancellationToken cancellationToken = default);
        Task<List<RbosfHabilidadeDto>> BuscarPorTermoAsync(string termo, string? classificacao = null, CancellationToken cancellationToken = default);
    }
}

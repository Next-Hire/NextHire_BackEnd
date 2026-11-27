using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface ITipoEntrevistaRepository
    {
        public Task<List<TipoEntrevista>> Listar();
        public Task<TipoEntrevista?> BuscarPorTipo(string tipo);
        public Task<TipoEntrevista?> BuscarPorId(int id);
    }
}

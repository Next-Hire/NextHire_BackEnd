using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface ISetor_OcupacaoRepository
    {

        public Task<IEnumerable<SetorOcupacao?>> ListagemTotal();
        public Task<IEnumerable<SetorOcupacao?>> ObterListagemSetoresPorEmpresaEPorArea(int? idEmpresa, string? empresaNome, string? area, int? areaId);
        public Task<IEnumerable<SetorOcupacao?>> ObterSetoresPorIdEmpresa(int idEmpresa);
        public Task<SetorOcupacao?> ObterSetorPorId(int id);
        public Task<SetorOcupacao?> ObterSetorPorNome(string nomeSetor);
        public Task<SetorOcupacao?> ObterSetorPorArea(int? idArea, string? area);
        public Task<SetorOcupacao?> ObterSetorPorAreaEmpresa(string? nomeEmpresa, int? idEmpresa, int? idArea, string? area, string nomeSetor);

        public Task<SetorOcupacao?> CadastrarSetor(int? idEmpresa, int? idArea, string? nomeEmpresa, string? nomeArea, string nomeSetor);
        public Task<SetorOcupacao?> AtualizarSetor(int idSetor, int idEmpresa, int idArea);
        public Task<SetorOcupacao?> ExcluirSetor(int idSetor);


    }
}

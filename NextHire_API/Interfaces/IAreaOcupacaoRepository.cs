using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IAreaOcupacaoRepository
    {
        List<AreaOcupacao> Listar();
        AreaOcupacao BuscarPorId(int id);
        AreaOcupacao BuscarPorNome(string nome);
        SetorOcupacao BuscarSetorPorId(int id);
        Usuario BuscarUsuarioPorId(int id);
        void Adicionar(AreaOcupacao areaOcupacao);
        void Atualizar(AreaOcupacao areaOcupacao);
        void Deletar(int id);

    }
}

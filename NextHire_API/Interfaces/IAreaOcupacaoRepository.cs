using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IAreaOcupacaoRepository
    {
        List<area_ocupacao> Listar();
        area_ocupacao BuscarPorId(int id);
        area_ocupacao BuscarPorNome(string nome);
        void Adicionar(area_ocupacao areaOcupacao);
        void Atualizar(area_ocupacao areaOcupacao);
        void Deletar(int id);

    }
}

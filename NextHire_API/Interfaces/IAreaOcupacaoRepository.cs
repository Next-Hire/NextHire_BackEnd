using NextHire_API.Domains;
using NextHire_API.DTOs.AreaOcupacaoDto;            

namespace NextHire_API.Interfaces
{
    public interface IAreaOcupacaoRepository
    {
        List<ListarAreaOcupacaoDto> Listar();
        AreaOcupacao BuscarPorId(int id);
        AreaOcupacao BuscarPorNome(string nome);
        Usuario BuscarUsuarioPorId(int id);
        void Adicionar(AreaOcupacao areaOcupacao);
        void Atualizar(int id, AreaOcupacao areaOcupacao);
        void Deletar(int id);

    }
}

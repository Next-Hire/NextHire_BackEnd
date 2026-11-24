using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IAnotacaoRepository
    {
        List<Anotacao> Listar();
        Anotacao BuscarPorId(int id);
        void Adicionar(Anotacao anotacao);
        void Atualizar(Anotacao anotacao);
        void Deletar(int id);
    }
}

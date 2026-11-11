using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IInformacaoCandidatoRepository
    {
        List<InformacaoCandidato> Listar();
        InformacaoCandidato BuscarPorId(int id);
        InformacaoCandidato BuscarPorCandidato(int candidatoId);
        void Adicionar(InformacaoCandidato informacaoCandidato);
        void Atualizar(int id, InformacaoCandidato informacaoCandidato);
        void Deletar(int id);
    }
}

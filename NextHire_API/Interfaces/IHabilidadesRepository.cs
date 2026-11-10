using NextHire_API.Domains;

namespace NextHire_API.Interfaces
{
    public interface IHabilidadesRepository
    {
        public List<Habilidade> Listar();
        public Habilidade BuscarPorId(int id);
        public void Adicionar(Habilidade novaHabilidade);
        public void Atualizar(int id, Habilidade habilidadeAtualizada);
        public void Deletar(int id);
    }
}

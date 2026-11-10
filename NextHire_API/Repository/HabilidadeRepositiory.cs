using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class HabilidadeRepositiory : IHabilidadesRepository
    {
        private readonly NextHireContext _context;
        public HabilidadeRepositiory(NextHireContext context)
        {
            _context = context;
        }

        public void Adicionar(Habilidade habilidade)
        {
            _context.Habilidade.Add(habilidade);
            _context.SaveChanges();
        }

        public void Atualizar(int id, Habilidade habilidadeAtualizada)
        {
            if(habilidadeAtualizada == null)
                return;

            Habilidade habilidadeExistente = _context.Habilidade.Find(id);
            if (habilidadeExistente == null)
                return;
            _context.Entry(habilidadeExistente).CurrentValues.SetValues(habilidadeAtualizada);
            _context.SaveChanges();
        }

        public Habilidade BuscarPorId(int id)
        {
            return _context.Habilidade.Find(id);
        }

        public void Deletar(int id)
        {
            Habilidade habilidadeExistente = _context.Habilidade.Find(id);
            if (habilidadeExistente == null)
                return;
            _context.Habilidade.Remove(habilidadeExistente);
            _context.SaveChanges();
        }

        public List<Habilidade> Listar()
        {
            return _context.Habilidade.ToList();
        }
    }
}

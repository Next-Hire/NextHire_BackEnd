using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class AnotacaoRepository : IAnotacaoRepository
    {
        private readonly NextHireContext _context;

        public AnotacaoRepository(NextHireContext context)
        {
            _context = context;
        }

        public void Adicionar(Anotacao anotacao)
        {
            _context.Anotacao.Add(anotacao);
            _context.SaveChanges();
        }

        public void Atualizar(Anotacao anotacao)
        {
            if(anotacao == null)
                return;

            Anotacao anotacaoExistente = _context.Anotacao.Find(anotacao.AnotacaoId);
            if (anotacaoExistente == null)
                return;

            _context.Entry(anotacaoExistente).CurrentValues.SetValues(anotacao);
            _context.SaveChanges();
        }

        public Anotacao BuscarPorId(int id)
        {
            return _context.Anotacao.FirstOrDefault(a => a.AnotacaoId == id);
        }

        public void Deletar(int id)
        {
            Anotacao anotacao = BuscarPorId(id);
            if (anotacao != null)
            {
                _context.Anotacao.Remove(anotacao);
                _context.SaveChanges();
            }
        }

        public List<Anotacao> Listar()
        {
            return _context.Anotacao.ToList();
        }
    }
}

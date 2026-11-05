using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class AreaOcupacaoRepository : IAreaOcupacaoRepository
    {
        private readonly NextHireContext _context;

        public AreaOcupacaoRepository(NextHireContext context)
        {
            _context = context;
        }

        public void Adicionar(AreaOcupacao areaOcupacao)
        {
            _context.AreaOcupacao.Add(areaOcupacao);
            _context.SaveChanges();
        }

        public void Atualizar(AreaOcupacao areaOcupacao)
        {
            if (areaOcupacao == null)
                return;

            AreaOcupacao area = _context.AreaOcupacao.Find(areaOcupacao.AreaOcupacaoId);
            if (area == null)
                return;

            _context.Entry(area).CurrentValues.SetValues(areaOcupacao);
            _context.SaveChanges();
        }

        public AreaOcupacao BuscarPorId(int id)
        {
            return _context.AreaOcupacao.Find(id);
        }

        public AreaOcupacao BuscarPorNome(string nome)
        {
            return _context.AreaOcupacao.FirstOrDefault(a => a.Nome == nome);
        }

        public void Deletar(int id)
        {
            AreaOcupacao area = _context.AreaOcupacao.Find(id);
            if (area == null)
                return;

            _context.AreaOcupacao.Remove(area);
            _context.SaveChanges();
        }

        public List<AreaOcupacao> Listar()
        {
            return _context.AreaOcupacao.ToList();
        }
    }
}

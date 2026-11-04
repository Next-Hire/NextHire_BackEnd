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

        public void Adicionar(area_ocupacao areaOcupacao)
        {
            _context.area_ocupacao.Add(areaOcupacao);
            _context.SaveChanges();
        }

        public void Atualizar(area_ocupacao areaOcupacao)
        {
            if (areaOcupacao == null)
                return;

            area_ocupacao area = _context.area_ocupacao.Find(areaOcupacao.area_ocupacao_id);
            if (area == null)
                return;

            _context.Entry(area).CurrentValues.SetValues(areaOcupacao);
            _context.SaveChanges();
        }

        public area_ocupacao BuscarPorId(int id)
        {
            return _context.area_ocupacao.Find(id);
        }

        public area_ocupacao BuscarPorNome(string nome)
        {
            return _context.area_ocupacao.FirstOrDefault(a => a.nome == nome);
        }

        public void Deletar(int id)
        {
            area_ocupacao area = _context.area_ocupacao.Find(id);
            if (area == null)
                return;

            _context.area_ocupacao.Remove(area);
            _context.SaveChanges();
        }

        public List<area_ocupacao> Listar()
        {
            return _context.area_ocupacao.ToList();
        }
    }
}

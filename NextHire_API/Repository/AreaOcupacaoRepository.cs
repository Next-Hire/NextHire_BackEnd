using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.DTOs.AreaOcupacaoDto;
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

        public void Atualizar(int id, AreaOcupacao areaOcupacao)
        {
            if (areaOcupacao == null)
                return;

            AreaOcupacao area = _context.AreaOcupacao.Find(id);
            if (area == null)
                return;

            _context.Entry(area).CurrentValues.SetValues(areaOcupacao);
            _context.SaveChanges();
        }

        public AreaOcupacao BuscarPorId(int id)
        {
            return _context.AreaOcupacao.FirstOrDefault(a => a.AreaOcupacaoId == id);
        }

        public AreaOcupacao BuscarPorNome(string nome)
        {
            return _context.AreaOcupacao.FirstOrDefault(a => a.Nome == nome);
        }


        public Usuario BuscarUsuarioPorId(int id)
        {
            return _context.Usuario.Find(id);
        }

        public void Deletar(int id)
        {
            AreaOcupacao area = _context.AreaOcupacao.Find(id);
            if (area == null)
                return;

            _context.AreaOcupacao.Remove(area);
            _context.SaveChanges();
        }

        public List<ListarAreaOcupacaoDto> Listar()
        {
            return _context.AreaOcupacao
                .Select(a => new ListarAreaOcupacaoDto
                {
                    AreaOcupacaoId = a.AreaOcupacaoId,
                    Nome = a.Nome
                })
                .ToList();
        }
    }
}

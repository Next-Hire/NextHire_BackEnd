using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class EmpresaRepository : IEmpresaRepository
    {

        private readonly NextHireContext _ctx;

        public EmpresaRepository(NextHireContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Empresa?>> Listar() 
            => await _ctx.Empresa.ToListAsync();
        public async Task<Empresa?> BuscarPorNome(string nome) =>
             await _ctx.Empresa.FirstOrDefaultAsync(varAux => varAux.Nome == nome) 
                    ?? null;

        public async Task<Empresa?> BuscarPorId(int id) =>
     await _ctx.Empresa.FindAsync(id)
            ?? null; // Descobrir como fazer com que o throw new InvalidOperationException()
                     // não pare a api

        public async Task<bool> EmpresaExiste(string nome)
            => await _ctx.Empresa.AnyAsync(varAux => varAux.Nome == nome);

        public async Task<Empresa?> Adicionar(Empresa EmpresaConstrutor)
        {
            await _ctx.Empresa.AddAsync(EmpresaConstrutor);
            await _ctx.SaveChangesAsync();
            return EmpresaConstrutor;
        }

        public async Task<Empresa?> Atualizar(int id, Empresa EmpresaConstrutor)
        {
            Empresa? EmpresaQuery = await _ctx.Empresa.FindAsync(id) 
                ?? null; // throw new InvalidOperationException()

            EmpresaQuery.Nome = EmpresaConstrutor.Nome;

            await _ctx.SaveChangesAsync();
            return EmpresaQuery;
        }


        public async Task<Empresa?> Remover(int id)
        {
            Empresa? EmpresaQuery = await _ctx.Empresa.FindAsync(id) 
                   ?? throw new InvalidOperationException("Empresa não encontrada");

            EmpresaQuery.StatusEmpresa = false;
            await _ctx.SaveChangesAsync();

            return EmpresaQuery;


        }

    }
}

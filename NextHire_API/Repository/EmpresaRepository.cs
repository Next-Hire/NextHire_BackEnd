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

        public async Task<IEnumerable<empresa?>> Listar() 
            => await _ctx.empresa.ToListAsync();
        public async Task<empresa?> BuscarPorNome(string nome) =>
             await _ctx.empresa.FirstOrDefaultAsync(varAux => varAux.nome == nome) 
                    ?? throw new InvalidOperationException("Empresa não encontrada");

        public async Task<empresa?> BuscarPorId(int id) =>
     await _ctx.empresa.FindAsync(id)
            ?? throw new InvalidOperationException("Empresa não encontrada");

        public async Task<bool?> empresaExiste(string nome)
            => await _ctx.empresa.AnyAsync(varAux => varAux.nome == nome);

        public async Task<empresa?> Adicionar(empresa EmpresaConstrutor)
        {
            await _ctx.empresa.AddAsync(EmpresaConstrutor);
            await _ctx.SaveChangesAsync();
            return EmpresaConstrutor;
        }

        public async Task<empresa?> Atualizar(int id, empresa empresaConstrutor)
        {
            empresa? empresaQuery = await _ctx.empresa.FindAsync(id) 
                ?? throw new InvalidOperationException("Empresa não encontrada");

            empresaQuery.nome = empresaConstrutor.nome;

            await _ctx.SaveChangesAsync();
            return empresaQuery;
        }


        public async Task<empresa?> Remover(int id)
        {
            empresa? empresaQuery = await _ctx.empresa.FindAsync(id) 
                   ?? throw new InvalidOperationException("Empresa não encontrada");

            empresaQuery.status_empresa = false;
            await _ctx.SaveChangesAsync();

            return empresaQuery;


        }

    }
}

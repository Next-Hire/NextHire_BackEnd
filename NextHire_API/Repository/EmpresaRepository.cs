using Microsoft.EntityFrameworkCore;
using NextHire_API.Applications.Services;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly NextHireContext _ctx;

        public EmpresaRepository(NextHireContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<empresa>> Listar()
         =>   await _ctx.empresa.ToListAsync() 
            ?? throw new InvalidOperationException("");
        

        public async Task<empresa?> BuscarNome(string nome) =>  
            await _ctx.empresa.FirstOrDefaultAsync(varAux => varAux.nome == nome);

        public async Task<empresa?> BuscarPorId(int empresaId)
        {
            //empresa empresaQuery = _ctx.empresa.FindAsync(empresaId);
            empresa empresaQuery = await _ctx.empresa.FindAsync(empresaId) 
                ?? throw new InvalidOperationException("Empresa não encontrada");
            return empresaQuery;
        }

        public async Task<bool> empresaExiste(string nome)
            => await _ctx.empresa.AnyAsync(varAux => varAux.nome == nome);

        public async Task<empresa?> Adicionar(empresa empresa)
        {
            await _ctx.empresa.AddAsync(empresa);
            await _ctx.SaveChangesAsync();
            return empresa;
        }


        public async Task<empresa?> Atualizar(int empresaId, empresa? empresa)
        {
            empresa? empresaQuery = await _ctx.empresa.FindAsync(empresaId) 
                    ?? throw new InvalidOperationException("Empresa não encontrada");
            _ctx.Entry(empresaQuery).CurrentValues.SetValues(empresa);
            await _ctx.SaveChangesAsync();
            return empresaQuery;
        }

        public async Task<empresa?> Remover(int empresaId)
        {
            empresa? empresaQuery = await _ctx.empresa.FindAsync(empresaId)
                ?? throw new InvalidOperationException("Empresa não encontrada");
            empresaQuery.status_empresa = false;
            await _ctx.SaveChangesAsync();
            return empresaQuery;
        }

 

    }
}

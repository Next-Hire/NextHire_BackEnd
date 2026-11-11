using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class Setor_OcupacaoRepository : ISetor_OcupacaoRepository
    {
        private readonly NextHireContext _ctx;
        public Setor_OcupacaoRepository(NextHireContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<SetorOcupacao?>> ListagemTotal() =>
            await _ctx.SetorOcupacao
                        .AsNoTracking()
                        .Include(varAux => varAux.Empresa)
                        .Include(varAux => varAux.AreaOcupacao)
                        .Include(varAux => varAux.Gestor)
                        .ToListAsync() ?? null;

        public async Task<List<SetorOcupacao?>> ObterListagemSetoresPorEmpresaEPorArea(int? idEmpresa, string? empresaNome, string? area, int? areaId)
        =>
           await _ctx.SetorOcupacao
                    .AsNoTracking()
                    .Include(varAux => varAux.Empresa)
                    .Include(varAux => varAux.AreaOcupacao)
                    .Include(varAux => varAux.Gestor)
                    .Where
                    (
                     varAux =>
                    (varAux.EmpresaId == idEmpresa || varAux.Empresa.Nome == empresaNome)
                    &&
                    (varAux.AreaOcupacao.Nome == area || varAux.AreaOcupacao.AreaOcupacaoId == areaId)
                    )
                    .ToListAsync() ?? null;


        public async Task<List<SetorOcupacao?>> ObterSetoresPorEmpresa(int? idEmpresa, string? empresaNome)
        =>
            await _ctx.SetorOcupacao
                .AsNoTracking()
                .Include(varAux => varAux.Empresa) // <-- Inclui a navegação da Empresa corretamente
                .Include(varAux => varAux.AreaOcupacao)
                .Include(varAux => varAux.Gestor)
                .Where(varAux => (varAux.EmpresaId == idEmpresa || varAux.Empresa.Nome == empresaNome)) // <-- Filtro mantido aqui
                .ToListAsync();
        
        /*
         
                     await _ctx.SetorOcupacao
               .AsNoTracking()
               .Include(varAux => varAux.Empresa) // <-- Inclui a navegação da Empresa corretamente
               .Include(varAux => varAux.AreaOcupacao)
               .Include(varAux => varAux.Gestor)
               .Where(varAux => (varAux.EmpresaId == idEmpresa || varAux.Empresa.Nome == empresaNome)) // <-- Filtro mantido aqui
               .ToListAsync() ?? null;
          
         */

        public async Task<SetorOcupacao?> ObterSetorPorId(int id)
            =>
                await _ctx.SetorOcupacao
                        .Include(varAux => varAux.Empresa)
                        .Include(varAux => varAux.AreaOcupacao)
                        .Include(varAux => varAux.Gestor)
                        .FirstOrDefaultAsync(varAux => varAux.SetorOcupacaoId == id) ?? null;


        public async Task<SetorOcupacao?> ObterSetorPorNome(string nomeSetor)
            =>
                await _ctx.SetorOcupacao
                        .Include(varAux => varAux.Empresa)
                        .Include(varAux => varAux.AreaOcupacao)
                        .Include(varAux => varAux.Gestor)
                        .FirstOrDefaultAsync(varAux => varAux.Nome == nomeSetor) ?? null;

        public async Task<SetorOcupacao?> ObterSetorPorArea(int? idArea, string? area)
            =>
                await _ctx.SetorOcupacao
                        .Include(varAux => varAux.Empresa)
                        .Include(varAux => varAux.AreaOcupacao)
                        .Include(varAux => varAux.Gestor)
                        .FirstOrDefaultAsync(varAux => (varAux.AreaOcupacaoId == idArea || varAux.AreaOcupacao.Nome == area)) ?? null;

        public async Task<SetorOcupacao?> ObterSetorPorAreaEmpresa(string? nomeEmpresa, int? idEmpresa, int? idArea, string? area, string nomeSetor)
            =>
                await _ctx.SetorOcupacao
                        .Include(varAux => varAux.Empresa)
                        .Include(varAux => varAux.AreaOcupacao)
                        .Include(varAux => varAux.Gestor)
                        .FirstOrDefaultAsync(varAux => (varAux.Empresa.Nome == nomeEmpresa || varAux.EmpresaId == idEmpresa) &&
                                                        (varAux.AreaOcupacao.Nome == area || varAux.AreaOcupacaoId == idArea) &&
                                                        (varAux.Nome == nomeSetor)) ?? null;
        public async Task<bool> existeSetor(string nomeSetor, int empresaId, int areaId)
            => 
            await _ctx.SetorOcupacao
                .Where(varAux => varAux.EmpresaId == empresaId && varAux.AreaOcupacaoId == areaId)
                .AnyAsync(varAux => varAux.Nome == nomeSetor);

        public async Task<SetorOcupacao?> CadastrarSetor(int? empresaId, int? areaId, string? nomeEmpresa, string? nomeArea, string? nomeSetor)
        {
            if (await _ctx.SetorOcupacao
                .Where(varAux => varAux.EmpresaId == empresaId && varAux.AreaOcupacaoId == areaId)
                .AnyAsync(varAux => varAux.Nome == nomeSetor))
                return null;


                var empresaQuery = await _ctx.Empresa.FirstOrDefaultAsync(varAux => (varAux.EmpresaId == empresaId || varAux.Nome == nomeEmpresa));
                    if(empresaQuery == null)
                        return null;

                var areaQuery = await _ctx.AreaOcupacao.FirstOrDefaultAsync(varAux => (varAux.AreaOcupacaoId == areaId || varAux.Nome == nomeArea));
                    if(areaQuery == null)
                        return null;

            SetorOcupacao setorOcup = new SetorOcupacao
            {
                EmpresaId = empresaQuery.EmpresaId,
                AreaOcupacaoId = areaQuery.AreaOcupacaoId,
                Nome = nomeSetor
            };

            await _ctx.SetorOcupacao.AddAsync(setorOcup);
            await _ctx.SaveChangesAsync();

            return setorOcup;
        }


        public async Task<SetorOcupacao?> AtualizarSetor(int idSetor, string? nomeEmpresa, string? nomeArea, List<string?> nomeGestor, string nomeSetor)
        {
            var Query = _ctx.SetorOcupacao.AsQueryable();

            if (nomeGestor != null)
                Query = Query.Include(varAux => varAux.Gestor);
            
            SetorOcupacao? setorOcupacaoQuery = 
                await Query.FirstOrDefaultAsync(varAux => varAux.SetorOcupacaoId == idSetor);
            
            Empresa? empresaQuery = 
                await _ctx.Empresa.FirstOrDefaultAsync(varAux => varAux.Nome == nomeEmpresa);

            AreaOcupacao? areaOcupacaoQuery = 
                await _ctx.AreaOcupacao.FirstOrDefaultAsync(varAux => varAux.Nome == nomeArea);



            if (setorOcupacaoQuery == null && (empresaQuery == null || areaOcupacaoQuery == null))
                return null;
            

            setorOcupacaoQuery.EmpresaId = empresaQuery.EmpresaId;
            setorOcupacaoQuery.Nome = nomeSetor;
            setorOcupacaoQuery.AreaOcupacaoId = areaOcupacaoQuery.AreaOcupacaoId;
            
            
            if (nomeGestor != null)
            {
                List<Usuario?> gestorQuery = 
                    await _ctx.Usuario
                        .Where(varAux => varAux.TipoUsuario.Equals("gestor") && nomeGestor.Contains(varAux.Nome))
                        .ToListAsync();

                List<Usuario?> listaDeletar = setorOcupacaoQuery.Gestor
                    .Where(varAux => !gestorQuery.Any(varAux2 => varAux2.UsuarioId == varAux2.UsuarioId))
                    .ToList();

                foreach (var gestor in listaDeletar)
                    setorOcupacaoQuery.Gestor.Remove(gestor);

                var listaAdicionar = gestorQuery.Where(varAux =>
                    !setorOcupacaoQuery.Gestor.Any(varAux2 => varAux2.UsuarioId == varAux2.UsuarioId)).ToList();
                
                foreach (var gestor in listaAdicionar)
                    setorOcupacaoQuery.Gestor.Add(gestor);

            }
            await _ctx.SaveChangesAsync();
            return setorOcupacaoQuery;

        } //
        
        
        public async Task<List<Usuario?>> ObterGestoresPorIds(List<int> gestoresIds)
        => 
             await _ctx.Usuario
                .Where(u => gestoresIds.Contains(u.UsuarioId))
                .ToListAsync();
        
        
        public async Task<SetorOcupacao?> Adicionar(SetorOcupacao setor)
        {
            await _ctx.SetorOcupacao.AddAsync(setor);
            await _ctx.SaveChangesAsync();

            return setor;
        }

        public async Task<SetorOcupacao?> Atualizar(SetorOcupacao setor)
        {
            _ctx.SetorOcupacao.Update(setor);
            await _ctx.SaveChangesAsync();

            return setor;
        }


        public async Task<SetorOcupacao?> ExcluirSetor(int setorId)
        {
            SetorOcupacao? querySetor = await _ctx.SetorOcupacao
                .Include(varAux => varAux.Gestor)
                .FirstOrDefaultAsync( varAux => varAux.SetorOcupacaoId == setorId);
            if(querySetor == null || querySetor.SetorOcupacaoId == 0)
                return null;


            querySetor.Gestor.Clear();
 
            _ctx.SetorOcupacao.Remove(querySetor);
            await _ctx.SaveChangesAsync();

            return querySetor;
 
        }

    }
}

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
                .Include(varAux => varAux.AreaOcupacao)
                .Include(varAux => varAux.Gestor)
                .Include(varAux => (varAux.Empresa.EmpresaId == idEmpresa || varAux.Empresa.Nome == empresaNome))
                .Where(varAux => (varAux.EmpresaId == idEmpresa || varAux.Empresa.Nome == empresaNome))
                .ToListAsync() ?? null;

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


        public async Task<SetorOcupacao?> AtualizarSetor(int idSetor, string? nomeEmpresa, string? nomeArea, string? nomeGestor, string nomeSetor)
        {
            SetorOcupacao? setorOcupacaoQuery = 
                await _ctx.SetorOcupacao.FindAsync(idSetor);

            Empresa? empresaQuery = 
                await _ctx.Empresa.FirstOrDefaultAsync(varAux => varAux.Nome == nomeEmpresa);

            AreaOcupacao? areaOcupacaoQuery = 
                await _ctx.AreaOcupacao.FirstOrDefaultAsync(varAux => varAux.Nome == nomeArea);

            Usuario? gestorQuery = 
                await _ctx.Usuario.FirstOrDefaultAsync(varAux => varAux.TipoUsuario.Equals("gestor") && varAux.Nome == nomeGestor);

            if (setorOcupacaoQuery == null && (empresaQuery == null || areaOcupacaoQuery == null || gestorQuery == null))
                return null;

            setorOcupacaoQuery.EmpresaId = empresaQuery.EmpresaId;
            setorOcupacaoQuery.Nome = nomeSetor;
            setorOcupacaoQuery.AreaOcupacaoId = areaOcupacaoQuery.AreaOcupacaoId;


            setorOcupacaoQuery.Gestor.Clear();
            setorOcupacaoQuery.Gestor.Add(gestorQuery);

            await _ctx.SaveChangesAsync();

            return setorOcupacaoQuery;

        }

    }
}

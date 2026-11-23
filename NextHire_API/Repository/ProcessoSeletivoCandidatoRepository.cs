using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Repository;
using NextHireAPI.Interfaces;
using NextHireAPI.Repository;

namespace NextHire_API.Interfaces;

public class ProcessoSeletivoCandidatoRepository : IProcessoSeletivoCandidatoRepository
{

    private readonly NextHireContext _ctx;
    private readonly UsuarioRepository _repUsuario;
    private readonly ProcessoSeletivoRepository _repProcessoSeletivo;
    public ProcessoSeletivoCandidatoRepository(NextHireContext ctx)
    {
        
    _ctx = ctx;
    
    } 
    
    public async Task<List<CandidatoProcessoSeletivo>> Listar()
        =>   await _ctx.CandidatoProcessoSeletivo
            .AsNoTracking()
            .Include(varAux => varAux.Candidato)
            .Include(varAux => varAux.ProcessoSeletivo)
            .ToListAsync();

    public async Task<List<CandidatoProcessoSeletivo>> ListarAprovados(int processoId)
    => await _ctx.CandidatoProcessoSeletivo
        .AsNoTracking()
        .Include(varAux => varAux.Candidato)
        .Include(varAux => varAux.ProcessoSeletivo)
        .Where(varAux => varAux.ProcessoSeletivoId == processoId && varAux.StatusRelacao == StatusRelacaoEnum.Avanca)
        .ToListAsync();

    public async Task<CandidatoProcessoSeletivo> BuscarPorId(int candidatoId, int processoSeletivoId)
        => await _ctx.CandidatoProcessoSeletivo
            .AsNoTracking()
            .Include(varAux => varAux.Candidato)
            .Include(varAux => varAux.ProcessoSeletivo)
            .FirstOrDefaultAsync(varAux =>
                varAux.ProcessoSeletivoId == processoSeletivoId && varAux.CandidatoId == candidatoId);

    public async Task<CandidatoProcessoSeletivo> BuscarPorProcessoSeletivoId(int processoSeletivoId)
        => await _ctx.CandidatoProcessoSeletivo
            .AsNoTracking()
            .Include(varAux => varAux.Candidato)
            .Include(varAux => varAux.ProcessoSeletivo)
            .FirstOrDefaultAsync(varAux => varAux.ProcessoSeletivoId == processoSeletivoId);
    
    public async Task<CandidatoProcessoSeletivo> BuscarPorCandidatoId(int candidatoId)
        => await _ctx.CandidatoProcessoSeletivo
            .AsNoTracking()
            .Include(varAux => varAux.Candidato)
            .Include(varAux => varAux.ProcessoSeletivo)
            .FirstOrDefaultAsync(varAux => varAux.CandidatoId == candidatoId);
    
    
    public async Task<CandidatoProcessoSeletivo> AdicionarCandidatoProcesso(CandidatoProcessoSeletivo candProcesso)
    {
        await _ctx.CandidatoProcessoSeletivo.AddAsync(candProcesso);
        await _ctx.SaveChangesAsync();
        return candProcesso;

    }




    public async Task<CandidatoProcessoSeletivo> AtualizarCandidatoProcesso(int candidatoId, int processoSeletivoId, CandidatoProcessoSeletivo candProcesso)
    {
        
        Usuario? candidatoQuery = await _ctx.Usuario
            .Where(varAux => varAux.TipoUsuario == TipoUsuarioEnum.Candidato)
            .FirstOrDefaultAsync(varAux => varAux.UsuarioId == candidatoId);
        ProcessoSeletivo? processoSeletivoQuery = await _ctx.ProcessoSeletivo.FindAsync(processoSeletivoId);
        
        if (candidatoQuery == null || processoSeletivoQuery == null)
            return null;
        
        CandidatoProcessoSeletivo? candidatoProcessoQuery = await _ctx.CandidatoProcessoSeletivo
            .FirstOrDefaultAsync(varAux => varAux.CandidatoId == candidatoQuery.UsuarioId && varAux.ProcessoSeletivoId == processoSeletivoQuery.ProcessoSeletivoId);

        if (candidatoProcessoQuery == null)
            return null;

        candidatoProcessoQuery.StatusRelacao = candProcesso.StatusRelacao;

        await _ctx.SaveChangesAsync();
        return candidatoProcessoQuery;

    }

    public async Task<CandidatoProcessoSeletivo> RemoverCandidatoProcesso(int candidatoId, int processoSeletivoId)
    {
        CandidatoProcessoSeletivo? candQuery = await _ctx.CandidatoProcessoSeletivo
            .FirstOrDefaultAsync(varAux =>
                varAux.CandidatoId == candidatoId && varAux.ProcessoSeletivoId == processoSeletivoId);

        if (candQuery == null)
            return null;

        _ctx.CandidatoProcessoSeletivo.Remove(candQuery);
        await _ctx.SaveChangesAsync();

        return candQuery;
    }

}
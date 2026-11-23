using NextHire_API.Interfaces;
using NextHireAPI.Interfaces;

namespace NextHire_API.Applications.Services;

public class CandidatoProcessoSeletivoService
{
    private readonly IUsuarioRepository _usuarioRep;
    private readonly IProcessoSeletivoRepository _processoSeletivoRepository;
    private readonly IProcessoSeletivoCandidatoRepository _rep;

    public CandidatoProcessoSeletivoService(IUsuarioRepository usuarioRep,
        IProcessoSeletivoRepository processoSeletivoRepository, IProcessoSeletivoCandidatoRepository rep)
    {
        _usuarioRep = usuarioRep;
        _processoSeletivoRepository = processoSeletivoRepository;
        _rep = rep;
    }
    
    
}
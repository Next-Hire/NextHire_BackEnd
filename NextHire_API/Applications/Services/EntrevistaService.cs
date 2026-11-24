using NextHire_API.Applications.Convertions.DomainToDto;
using NextHire_API.Domains;
using NextHire_API.DTOs.EntrevistaDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;
using NextHireAPI.Interfaces;

namespace NextHire_API.Applications.Services;

public class EntrevistaService
{
    private readonly IEntrevistaRepository _rep;
    // private readonly IProcessoSeletivoRepository _ProcessoRep;
    private readonly IUsuarioRepository _usuarioRep;

    public EntrevistaService(IEntrevistaRepository rep,  IUsuarioRepository usuarioRep)
    {
        _rep = rep;
        // _ProcessoRep = ProcessoRep;
        _usuarioRep = usuarioRep;
    }


    public async Task<List<LerEntrevistaDTO>> Listar()
    {
        List<Entrevista> e = await _rep.Listar();
        if (e == null)
            throw new DomainException("Nenhuma entrevista encontrada");
        
        return e.Select(varAux => EntrevistaToDTO.LerDTO(varAux)).ToList();

    }

    public async Task<List<LerEntrevistaDTO>> EntrevistasHoje()
    {
        DateTime hoje = DateTime.Today;
        

        List<Entrevista> e = await _rep.EntrevistasHoje(hoje);
        
        return e.Select(varAux => EntrevistaToDTO.LerDTO(varAux)).ToList();
    }
    
    
    public async Task<List<LerEntrevistaDTO>> BuscarPorCandidato(int candidatoId)
{
    if (candidatoId <= 0)
        throw new DomainException("ID do candidato inválido.");

    List<Entrevista> entrevistas = await _rep.BuscarPorCandidato(candidatoId);

    return entrevistas.Select(e => EntrevistaToDTO.LerDTO(e)).ToList();
}

public async Task<LerEntrevistaDTO> BuscarPorId(int id)
{
    if (id <= 0)
        throw new DomainException("ID da entrevista inválido.");

    Entrevista? entrevista = await _rep.BuscarPorId(id);
    if (entrevista == null)
        throw new DomainException("Entrevista não encontrada.");

    return EntrevistaToDTO.LerDTO(entrevista);
}

public async Task<LerEntrevistaDTO> AdicionarEntrevista(CriarEntrevistaDTO dto)
{
    if (string.IsNullOrWhiteSpace(dto.tituloEntrevista))
        throw new DomainException("O título da entrevista é obrigatório.");

    if (dto.dataEncerra <= dto.dataInicio)
        throw new DomainException("A data/hora de encerramento deve ser posterior à data/hora de início.");

    if (dto.ProcessoSeletivoId <= 0)
        throw new DomainException("Processo seletivo inválido.");

    DateTime inicio = DateTime.SpecifyKind(dto.dataInicio, DateTimeKind.Unspecified);
    DateTime fim = DateTime.SpecifyKind(dto.dataEncerra, DateTimeKind.Unspecified);

    Entrevista novaEntrevista = new Entrevista
    {
        NomeEntrevista = dto.tituloEntrevista,
        HorarioInicio = inicio,
        HorarioFim = fim,
        ProcessoSeletivoId = dto.ProcessoSeletivoId,
        StatusEntrevista = true
    };
    List<Usuario> candidatosQuery = await _usuarioRep.ListarCandidatos();
    List<int> candidatosQueryIds = candidatosQuery.Select(c => c.UsuarioId).ToList();

    List<int> idsUnicos = dto.CandidatosIds?.Distinct().ToList() ?? new List<int>();

// Verifica se algum ID enviado no DTO não existe na lista de candidatos válidos
    bool existemIdsInvalidos = idsUnicos.Except(candidatosQueryIds).Any();
    if (existemIdsInvalidos)
        throw new DomainException("Um ou mais candidatos informados não foram encontrados.");

    Entrevista entrevistaSalva = await _rep.AdicionarEntrevista(novaEntrevista, idsUnicos);
 

    return EntrevistaToDTO.LerDTO(entrevistaSalva);
}

public async Task<LerEntrevistaDTO> AtualizarCandidatoNaEntrevista(int entrevistaId, int candidatoId)
{
    if (entrevistaId <= 0 || candidatoId <= 0)
        throw new DomainException("IDs de entrevista e candidato devem ser válidos.");
    
    Entrevista? entrevistaAtualizada = await _rep.AtualizarCandidatoNaEntrevista(entrevistaId, candidatoId);
    if (entrevistaAtualizada == null)
        throw new DomainException("Entrevista ou candidato não encontrado.");

    return EntrevistaToDTO.LerDTO(entrevistaAtualizada);
}

public async Task<LerEntrevistaDTO> AtualizarDataDaEntrevista(int entrevistaId, DateTime? dataInicio, DateTime? dataFinal)
{
    if (entrevistaId <= 0)
        throw new DomainException("ID da entrevista inválido.");

    if (!dataInicio.HasValue && !dataFinal.HasValue)
        throw new DomainException("Informe ao menos a data de início ou a data final.");

    if (dataInicio.HasValue && dataFinal.HasValue && dataFinal.Value <= dataInicio.Value)
        throw new DomainException("A data/hora final deve ser maior que a data/hora inicial.");

    DateTime? inicio = dataInicio.HasValue
        ? DateTime.SpecifyKind(dataInicio.Value, DateTimeKind.Unspecified)
        : null;

    DateTime? fim = dataFinal.HasValue
        ? DateTime.SpecifyKind(dataFinal.Value, DateTimeKind.Unspecified)
        : null;

    Entrevista? entrevistaAtualizada = await _rep.AtualizarDataDaEntrevista(entrevistaId, inicio, fim);
    if (entrevistaAtualizada == null)
        throw new DomainException("Entrevista não encontrada.");

    return EntrevistaToDTO.LerDTO(entrevistaAtualizada);
}

public async Task<LerEntrevistaDTO> RemoverEntrevista(int id)
{
    if (id <= 0)
        throw new DomainException("ID da entrevista inválido.");

    Entrevista? entrevistaRemovida = await _rep.RemoverEntrevista(id);
    if (entrevistaRemovida == null)
        throw new DomainException("Entrevista não encontrada.");

    return EntrevistaToDTO.LerDTO(entrevistaRemovida);
}
    
    
}
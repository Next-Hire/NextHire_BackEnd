using NextHire_API.Applications.Convertions.DomainToDTO;
using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoProcessoSeletivoDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;
using NextHireAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using NextHire_API.Applications.Email;

namespace NextHire_API.Applications.Services;

public class CandidatoProcessoSeletivoService
{
    private readonly IUsuarioRepository _usuarioRep;
    private readonly IProcessoSeletivoRepository _processoSeletivoRepository;
    private readonly IProcessoSeletivoCandidatoRepository _rep;
    private readonly NotificationService _notifications;

    public CandidatoProcessoSeletivoService(IUsuarioRepository usuarioRep,
        IProcessoSeletivoRepository processoSeletivoRepository, IProcessoSeletivoCandidatoRepository rep,
        NotificationService notifications)
    {
        _usuarioRep = usuarioRep;
        _processoSeletivoRepository = processoSeletivoRepository;
        _rep = rep;
        _notifications = notifications;
    }
    
    public async Task<List<LerCandidatoProcessoDTO>> Listar()
    {
        List<CandidatoProcessoSeletivo> lista = await _rep.Listar();
        return lista.Select(CandidatoProcessoSeletivoToDTO.LerDTO).ToList();
    }
    
    public async Task<List<LerCandidatoProcessoDTO>> ListarAprovados(int processoId)
    {
        if (processoId <= 0)
            throw new DomainException("ID do processo seletivo inválido.");

        ProcessoSeletivo? processo = await _processoSeletivoRepository.BuscarProcessoSeletivoPorId(processoId);
        if (processo == null)
            throw new DomainException("Processo seletivo não encontrado.");

        List<CandidatoProcessoSeletivo> aprovadosQuery = await _rep.ListarAprovados(processoId);
        return aprovadosQuery.Select(CandidatoProcessoSeletivoToDTO.LerDTO).ToList();
    }

    public async Task<LerCandidatoProcessoDTO> BuscarPorId(int candidatoId, int processoSeletivoId)
    {
        if (candidatoId <= 0 || processoSeletivoId <= 0)
            throw new DomainException("IDs de candidato e processo seletivo devem ser válidos.");

        CandidatoProcessoSeletivo? relacao = await _rep.BuscarPorId(candidatoId, processoSeletivoId)
            ?? throw new DomainException("Vínculo entre candidato e processo seletivo não encontrado.");

        return CandidatoProcessoSeletivoToDTO.LerDTO(relacao);
    }

    public async Task<LerCandidatoProcessoDTO> BuscarPorProcessoSeletivoId(int processoSeletivoId)
    {
        if (processoSeletivoId <= 0)
            throw new DomainException("ID do processo seletivo inválido.");

        CandidatoProcessoSeletivo? relacao = await _rep.BuscarPorProcessoSeletivoId(processoSeletivoId)
            ?? throw new DomainException("Nenhum candidato encontrado para este processo seletivo.");

        return CandidatoProcessoSeletivoToDTO.LerDTO(relacao);
    }

    public async Task<LerCandidatoProcessoDTO> BuscarPorCandidatoId(int candidatoId)
    {
        if (candidatoId <= 0)
            throw new DomainException("ID do candidato inválido.");

        CandidatoProcessoSeletivo? relacao = await _rep.BuscarPorCandidatoId(candidatoId)
            ?? throw new DomainException("Nenhum processo seletivo encontrado para este candidato.");

        return CandidatoProcessoSeletivoToDTO.LerDTO(relacao);
    }

    public async Task<LerCandidatoProcessoDTO> AdicionarCandidatoProcesso(CriarCandidatoProcessoDTO dto)
    {
        if (dto == null)
            throw new DomainException("Os dados do vínculo são obrigatórios.");

        if (dto.CandidatoId <= 0 || dto.ProcessoSeletivoId <= 0)
            throw new DomainException("IDs de candidato e processo seletivo devem ser válidos.");

        // caso não possua nenhuma relacao com os enums ja cadastrados 
        if (!Enum.IsDefined(typeof(StatusRelacaoEnum), dto.StatusRelacao))
            throw new DomainException("Status da relação inválido.");

        Usuario? usuario = await _usuarioRep.BuscarCandidatoPorId(dto.CandidatoId)
            ?? throw new DomainException("Candidato não encontrado.");

        if (usuario.TipoUsuario != TipoUsuarioEnum.Candidato)
            throw new DomainException("O usuário informado não possui perfil de candidato.");

        ProcessoSeletivo? processo = await _processoSeletivoRepository.BuscarProcessoSeletivoPorId(dto.ProcessoSeletivoId)
            ?? throw new DomainException("Processo seletivo não encontrado.");

        if (processo.StatusProcessoSeletivo == false)
            throw new DomainException("Não é possível vincular candidatos a um processo seletivo já encerrado.");

        CandidatoProcessoSeletivo? relacaoExistente = await _rep.BuscarPorId(dto.CandidatoId, dto.ProcessoSeletivoId);
        if (relacaoExistente != null)
            throw new DomainException("Este candidato já está vinculado a este processo seletivo.");

        CandidatoProcessoSeletivo novaRelacao = new CandidatoProcessoSeletivo
        {
            CandidatoId = dto.CandidatoId,
            ProcessoSeletivoId = dto.ProcessoSeletivoId,
            StatusRelacao = dto.StatusRelacao
        };

        await _rep.AdicionarCandidatoProcesso(novaRelacao);

        try { await _notifications.StatusAtualizado(dto.CandidatoId, dto.ProcessoSeletivoId, dto.StatusRelacao); }
        catch (Exception) { }

        // Atribui as navegações já carregadas para retornar nomeCandidato e nomeProcesso preenchidos
        novaRelacao.Candidato = usuario;
        novaRelacao.ProcessoSeletivo = processo;

        return CandidatoProcessoSeletivoToDTO.LerDTO(novaRelacao);
    }
    
    public async Task<List<LerCandidatoProcessoDTO>> BuscarCandidatosPorProcessoSeletivoId(int processoSeletivoId)
    {
        if (processoSeletivoId <= 0)
            throw new DomainException("O ID do processo seletivo deve ser maior que zero.");

        List<CandidatoProcessoSeletivo> relacoes = await _rep.ListarCandidatosDoProcesso(processoSeletivoId);

        if (relacoes == null || relacoes.Count == 0)
            throw new DomainException("Nenhum candidato encontrado para o processo seletivo informado.");

        return relacoes.Select(CandidatoProcessoSeletivoToDTO.LerDTO).ToList();
    }
    
    public async Task<List<LerCandidatoProcessoDTO>> AdicionarCandidatosNoProcessoSeletivo(CriarCandidatosNoProcessoSeletivoDTO dto)
    {
        if (dto == null)
            throw new DomainException("Os dados para vínculo em lote são obrigatórios.");

        if (dto.ProcessoSeletivoId <= 0)
            throw new DomainException("O ID do processo seletivo deve ser maior que zero.");

        if (dto.CandidatosIds == null || dto.CandidatosIds.Count == 0)
            throw new DomainException("Informe ao menos um ID de candidato.");
                                                // 
        List<int> idsUnicos = dto.CandidatosIds.Distinct().ToList();

        if (idsUnicos.Any(id => id <= 0))
            throw new DomainException("Todos os IDs de candidatos devem ser maiores que zero.");

        if (!Enum.IsDefined(typeof(StatusRelacaoEnum), dto.StatusRelacao))
            throw new DomainException("O status da relação informado é inválido.");

        // Verifica se algum dos candidatos já está vinculado ao processo seletivo
        List<CandidatoProcessoSeletivo> existentes = await _rep.ListarCandidatosDoProcesso(dto.ProcessoSeletivoId);
        List<int> idsJaVinculados = existentes
            .Where(e => idsUnicos.Contains(e.CandidatoId))
            .Select(e => e.CandidatoId)
            .ToList();
            

        if (idsJaVinculados.Count > 0)
            throw new DomainException($"Os seguintes candidatos já estão vinculados a este processo seletivo: {string.Join(", ", idsJaVinculados)}.");

        List<CandidatoProcessoSeletivo> novasRelacoes = idsUnicos.Select(candidatoId => new CandidatoProcessoSeletivo
        {
            ProcessoSeletivoId = dto.ProcessoSeletivoId,
            CandidatoId = candidatoId,
            StatusRelacao = dto.StatusRelacao
        }).ToList();

        await _rep.AdicionarCandidatosNoProcesso(novasRelacoes);

        foreach (CandidatoProcessoSeletivo relacao in novasRelacoes)
        {
            try { await _notifications.StatusAtualizado(relacao.CandidatoId, relacao.ProcessoSeletivoId, relacao.StatusRelacao); }
            catch (Exception) { }
        }

        return novasRelacoes.Select(CandidatoProcessoSeletivoToDTO.LerDTO).ToList();
    }

    public async Task<LerCandidatoProcessoDTO> AtualizarCandidatoProcesso(
        int candidatoId,
        int processoSeletivoId,
        AtualizarCandidatoProcessoDTO dto)
    {
        if (candidatoId <= 0 || processoSeletivoId <= 0)
            throw new DomainException("IDs de candidato e processo seletivo devem ser válidos.");

        if (dto == null)
            throw new DomainException("Os dados para atualização são obrigatórios.");

        if (!Enum.IsDefined(typeof(StatusRelacaoEnum), dto.statusRelacao))
            throw new DomainException("Status da relação inválido.");

        CandidatoProcessoSeletivo dadosAtualizacao = new CandidatoProcessoSeletivo
        {
            StatusRelacao = dto.statusRelacao
        };

        CandidatoProcessoSeletivo? atualizado = await _rep.AtualizarCandidatoProcesso(
            candidatoId,
            processoSeletivoId,
            dadosAtualizacao);

        if (atualizado == null)
            throw new DomainException("Vínculo entre candidato e processo seletivo não encontrado.");

        // Recarrega via BuscarPorId (que possui .Include) para garantir nomeProcesso e nomeCandidato no DTO
        CandidatoProcessoSeletivo completo = await _rep.BuscarPorId(candidatoId, processoSeletivoId) ?? atualizado;

        try { await _notifications.StatusAtualizado(candidatoId, processoSeletivoId, dto.statusRelacao); }
        catch (Exception) { }

        return CandidatoProcessoSeletivoToDTO.LerDTO(completo);
    }

    public async Task<LerCandidatoProcessoDTO> RemoverCandidatoProcesso(int candidatoId, int processoSeletivoId)
    {
        if (candidatoId <= 0 || processoSeletivoId <= 0)
            throw new DomainException("IDs de candidato e processo seletivo devem ser válidos.");

        // Busca antes de remover para preservar os nomes (Candidato e ProcessoSeletivo) no retorno
        CandidatoProcessoSeletivo? relacaoExistente = await _rep.BuscarPorId(candidatoId, processoSeletivoId)
            ?? throw new DomainException("Vínculo entre candidato e processo seletivo não encontrado.");

        await _rep.RemoverCandidatoProcesso(candidatoId, processoSeletivoId);

        return CandidatoProcessoSeletivoToDTO.LerDTO(relacaoExistente);
    }
}
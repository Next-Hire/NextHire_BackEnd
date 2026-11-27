using NextHire_API.Applications.Convertions;
using NextHire_API.Applications.Convertions.DomainToDto;
using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoVagaDTO;
using NextHire_API.DTOs.VagaDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;
using NextHireAPI.Domains;
using NextHire_API.Applications.Email;
using System.Runtime.CompilerServices;

namespace NextHire_API.Applications.Services
{
    public class CurriculoVagaService
    {
        private readonly ICurriculoVagaRepository _repository;
        private readonly NotificationService _notifications;

        public CurriculoVagaService(ICurriculoVagaRepository repository, NotificationService notifications)
        {
            _repository = repository;
            _notifications = notifications;
        }

        public async Task<List<ListarCurriculoVagaDto>> ListarTodasAsCandidaturasPorEmpresaId(int empresaId)
        {
            List<CurriculoVaga> candidaturas = await _repository.ListarTodasAsCandidaturasPorEmpresaId(empresaId);
            if(candidaturas == null || candidaturas.Count == 0)
                throw new DomainException("Nenhum candidato encontrado para a vaga informada.");

            return candidaturas.Select(c => CurriculoVagaToDTO.CurriculoVagaToDto(c)).ToList();
        }

        public async Task<List<ListarCurriculoVagaDto>> ListarCandidatosPorVagaId(int vagaId)
        {
            List<CurriculoVaga> candidaturas = await _repository.ListarCandidatosPorVagaId(vagaId);
            if(candidaturas == null || candidaturas.Count == 0)
                throw new DomainException("Nenhum candidato encontrado para a vaga informada.");

            return candidaturas.Select(c => CurriculoVagaToDTO.CurriculoVagaToDto(c)).ToList();
        }

        public async Task<List<ListarVagaDto>> ListarVagaPorCandidatoId(int candidatoId)
        {
            List<Vaga> vagas = await _repository.ListarVagasPorCandidatoId(candidatoId);
            if (vagas == null || vagas.Count <= 0)
                throw new DomainException("Nenhuma vaga encontrada.");

            return vagas.Select(v => VagaToDTO.VagaToDto(v)).ToList();
        }

        public async Task EfetuarCandidatura(int curriculoId, int vagaId)
        {
            if (curriculoId <= 0)
                throw new DomainException("Selecione um curriculo para prosseguir");

            if (vagaId <= 0)
                throw new DomainException("Vaga não encontrada");

            //TODO Validar possiveis nulos para a query
            await _repository.CadastrarCurriculoVaga(curriculoId, vagaId);

            try { await _notifications.CandidaturaRecebida(curriculoId, vagaId); }
            catch (Exception) { }
        }

        public async Task CancelarCandidatura(int curriculoId, int vagaId)
        {
            if (curriculoId <= 0)
                throw new DomainException("Selecione um curriculo para prosseguir");

            if (vagaId <= 0)
                throw new DomainException("Vaga não encontrada");

            try { await _notifications.CandidaturaCancelada(curriculoId, vagaId); }
            catch (Exception) { }

            await _repository.Remover(curriculoId, vagaId);
        }
    }
}

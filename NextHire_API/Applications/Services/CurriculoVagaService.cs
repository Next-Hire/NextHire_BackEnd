using NextHire_API.Applications.Convertions;
using NextHire_API.Applications.Convertions.DomainToDto;
using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoVagaDTO;
using NextHire_API.DTOs.VagaDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;
using NextHireAPI.Domains;
using System.Runtime.CompilerServices;

namespace NextHire_API.Applications.Services
{
    public class CurriculoVagaService
    {
        private readonly ICurriculoVagaRepository _repository;

        public CurriculoVagaService(ICurriculoVagaRepository repository) => _repository = repository;

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
        }

        public async Task CancelarCandidatura(int curriculoId, int vagaId)
        {
            if (curriculoId <= 0)
                throw new DomainException("Selecione um curriculo para prosseguir");

            if (vagaId <= 0)
                throw new DomainException("Vaga não encontrada");

            await _repository.Remover(curriculoId, vagaId);
        }
    }
}

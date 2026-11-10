using NextHire_API.Applications.Convertions;
using NextHire_API.Domains;
using NextHire_API.DTOs.VagaDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHireAPI.Applications.Services
{
    public class VagaService
    {
        private readonly IVagaRepository _repository;

        public VagaService(IVagaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ListarVagaDto>> Listar()
        {
            List<Vaga> vagas = await _repository.Listar();

            if (vagas.Count <= 0 || vagas == null)
                throw new DomainException("Nenhuma vaga encontrada.");

            return vagas.Select(v => VagaToDTO.VagaToDto(v)).ToList();
        }

        public async Task<List<ListarVagaDto>> ListarPorEmpresaId(int empresaId)
        {
            if (empresaId <= 0)
                throw new DomainException("Informe um ID de empresa válido.");

            List<Vaga> vagas = await _repository.ListarPorEmpresaId(empresaId);

            if (vagas.Count <= 0 || vagas == null)
                throw new DomainException("Nenhuma vaga encontrada com este ID de empresa.");

            return vagas.Select(v => VagaToDTO.VagaToDto(v)).ToList();
        }

        public async Task<List<ListarVagaDto>> ListarPorTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new DomainException("Informe um título válido.");

            List<Vaga> vagas = await _repository.ListarPorTitulo(titulo);

            if (vagas.Count <= 0 || vagas == null)
                throw new DomainException("Nenhuma vaga encontrada com este título.");

            return vagas.Select(v => VagaToDTO.VagaToDto(v)).ToList();
        }

        public async Task<List<ListarVagaDto>> ListarPorStatus(bool status)
        {
            List<Vaga> vagas = await _repository.ListarPorStatus(status);

            if (vagas.Count <= 0 || vagas == null)
                throw new DomainException("Nenhuma vaga encontrada com este status.");

            return vagas.Select(v => VagaToDTO.VagaToDto(v)).ToList();
        }

        public async Task<ListarVagaDto> BuscarPorId(int vagaId)
        {
            if (vagaId <= 0)
                throw new DomainException("Informe um ID de vaga válido.");

            return VagaToDTO.VagaToDto(await _repository.BuscarPorId(vagaId)
                ?? throw new DomainException("Vaga não encontrada."));
        }

        public async Task Adicionar(AdicionarVagaDto vaga)
        {
            if (vaga == null)
                throw new DomainException("Informe os dados da vaga.");

            if (string.IsNullOrWhiteSpace(vaga.Titulo))
                throw new DomainException("O título da vaga é obrigatório.");

            if (string.IsNullOrWhiteSpace(vaga.Descricao))
                throw new DomainException("A descrição da vaga é obrigatória.");

            if (vaga.EmpresaId <= 0)
                throw new DomainException("Informe uma empresa válida.");

            await _repository.Adicionar(VagaToDTO.AdicionarVagaDtoToDomain(vaga));
        }

        public async Task Atualizar(int vagaId, AtualizarVagaDto vaga)
        {
            if (vagaId <= 0)
                throw new DomainException("Informe um ID de vaga válido.");

            if (vaga == null)
                throw new DomainException("Informe os dados para atualização.");

            await _repository.Atualizar(vagaId, VagaToDTO.AtualizarVagaDtoToDomain(vaga));
        }

        public async Task Remover(int vagaId)
        {
            if (vagaId <= 0)
                throw new DomainException("Informe um ID de vaga válido.");

            await _repository.Remover(vagaId);
        }
    }
}
using NextHire_API.Convertions.DomainToDTO;
using NextHire_API.Domains;
using NextHire_API.DTOs.ProcessoSeletivoDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class ProcessoSeletivoService
    {
        private readonly IProcessoSeletivoRepository _repository;
        private readonly IVagaRepository _vagaRepository;

        public ProcessoSeletivoService(IProcessoSeletivoRepository repository, IVagaRepository vagaRepository)
        {
            _repository = repository;
            _vagaRepository = vagaRepository;
        }

        public async Task<List<ListarProcessoSeletivoDto>> ListarProcessosSeletivos()
        {
            List<ProcessoSeletivo> processos = await _repository.ListarProcessosSeletivos();
            if (processos.Count <= 0 || processos == null)
                throw new DomainException("Nenhum processo seletivo encontrado.");

            return processos.Select(p => ProcessoSeletivoToDto.ConvertProcessoSeletivoToDto(p)).ToList();
        }

        public async Task<List<ListarProcessoSeletivoDto>> ListarProcessosSeletivosPorEmpresaId(int empresaId)
        {
            List<ProcessoSeletivo> processos = await _repository.ListarProcessosSeletivosPorEmpresaId(empresaId);
            if (processos.Count <= 0 || processos == null)
                throw new DomainException("Nenhum processo seletivo encontrado.");

            return processos.Select(p => ProcessoSeletivoToDto.ConvertProcessoSeletivoToDto(p)).ToList();
        }

        public async Task<List<ListarProcessoSeletivoDto>> ListarProcessosSeletivosPorSetorId(int setorId)
        {
            List<ProcessoSeletivo> processos = await _repository.ListarProcessosSeletivosPorSetorId(setorId);
            if (processos.Count <= 0 || processos == null)
                throw new DomainException("Nenhum processo seletivo encontrado.");

            return processos.Select(p => ProcessoSeletivoToDto.ConvertProcessoSeletivoToDto(p)).ToList();
        }

        public async Task<List<ListarProcessoSeletivoDto>> ListarProcessosSeletivosPorAreaId(int areaId)
        {
            List<ProcessoSeletivo> processos = await _repository.ListarProcessosSeletivosPorAreaId(areaId);
            if (processos.Count <= 0 || processos == null)
                throw new DomainException("Nenhum processo seletivo encontrado.");

            return processos.Select(p => ProcessoSeletivoToDto.ConvertProcessoSeletivoToDto(p)).ToList();
        }

        public async Task<ListarProcessoSeletivoDto> BuscarProcessoSeletivoPorId(int processoId)
        {
            ProcessoSeletivo processo = await _repository.BuscarProcessoSeletivoPorId(processoId);
            if (processo == null)
                throw new DomainException("processo seletivo não encontrado.");

            return ProcessoSeletivoToDto.ConvertProcessoSeletivoToDto(processo);
        }

        public async Task AdicionarProcessoSeletivo(AdicionarProcessoSeletivoDto processoSeletivo)
        {
            if (processoSeletivo.DataFechamento <= DateOnly.FromDateTime(DateTime.Today))
                throw new DomainException("A data de fechamento deve ser posterior a data atual.");

            if (string.IsNullOrWhiteSpace(processoSeletivo.TituloProcesso))
                throw new DomainException("O titulo do processo seletivo é obrigatório");

            if (processoSeletivo.VagaId <= 0)
                throw new DomainException("Digite um ID valido para a vaga");

            Vaga vaga = await _vagaRepository.BuscarPorId(processoSeletivo.VagaId);

            if (vaga == null)
                throw new DomainException("A vaga indicada não existe!");

            await _repository.AdicionarProcessoSeletivo(ProcessoSeletivoToDto.AdicionarProcessoSeletivoToDomain(processoSeletivo));
        }

        public async Task AtualizarProcessoSeletivo(int processoId, string? nomeProcesso, DateOnly? DataFechamento)
        {
            if (string.IsNullOrWhiteSpace(nomeProcesso) && DataFechamento == null)
                throw new DomainException("Pelo menos um campos é obrigatório");

            if (DataFechamento.HasValue && DataFechamento.Value <= DateOnly.FromDateTime(DateTime.Today))
                throw new DomainException("A data e/ou hora de fechamento não pode estar no passado.");

            ProcessoSeletivo processo = await _repository.BuscarProcessoSeletivoPorId(processoId);

            await _repository.AtualizarProcessoSeletivo(ProcessoSeletivoToDto.AtualizarProcessoSeletivoToDomain(processo, nomeProcesso, DataFechamento));
        }

        public async Task<bool> RemoverProcessoSeletivo(int processoId)
            => await _repository.RemoverProcessoSeletivo(processoId);
    }
}
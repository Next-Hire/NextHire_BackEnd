using NextHire_API.Domains;
using NextHire_API.DTOs.AvaliacaoHabilidadeDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class AvaliacaoHabilidadeService
    {
        private readonly IAvaliacaoHabilidadeRepository _repository;

        public AvaliacaoHabilidadeService(IAvaliacaoHabilidadeRepository repository)
        {
            _repository = repository;
        }

        private static ListarAvaliacaoHabilidadeDto ConverterParaDto(AvaliacaoHabilidade avaliacao)
        {
            return new ListarAvaliacaoHabilidadeDto
            {
                CandidatoId = avaliacao.CandidatoId,
                NomeCandidato = avaliacao.Candidato?.Nome ?? string.Empty,
                EmailCandidato = avaliacao.Candidato?.Email ?? string.Empty,

                EntrevistaId = avaliacao.EntrevistaId,
                NomeEntrevista = avaliacao.Entrevista?.NomeEntrevista ?? string.Empty,
                HorarioInicio = DateOnly.FromDateTime(avaliacao.Entrevista.HorarioInicio),
                HorarioFim = DateOnly.FromDateTime(avaliacao.Entrevista?.HorarioFim ?? default),

                HabilidadeId = avaliacao.HabilidadeId,
                NomeHabilidade = avaliacao.Habilidade?.Nome ?? string.Empty,
                NivelAvaliado = avaliacao.NivelAvaliado
            };
        }

        private static void ValidarChave(int candidatoId, int entrevistaId, int habilidadeId)
        {
            if (candidatoId <= 0)
                throw new DomainException("O ID do candidato deve ser maior que zero.");
            if (entrevistaId <= 0)
                throw new DomainException("O ID da entrevista deve ser maior que zero.");
            if (habilidadeId <= 0)
                throw new DomainException("O ID da habilidade deve ser maior que zero.");
        }

        private static void ValidarNivel(int nivelAvaliado)
        {
            if (nivelAvaliado <= 0)
                throw new DomainException("O nível avaliado deve ser maior que zero.");
        }

        public async Task<List<ListarAvaliacaoHabilidadeDto>> Listar()
        {
            var avaliacoes = await _repository.Listar();
            return avaliacoes.Select(ConverterParaDto).ToList();
        }

        public async Task<ListarAvaliacaoHabilidadeDto> BuscarPorChave(
            int candidatoId, int entrevistaId, int habilidadeId)
        {
            ValidarChave(candidatoId, entrevistaId, habilidadeId);

            var avaliacao = await _repository.BuscarPorId(candidatoId, entrevistaId, habilidadeId);
            if (avaliacao == null)
                throw new KeyNotFoundException("Avaliação de habilidade não encontrada.");

            return ConverterParaDto(avaliacao);
        }

        public async Task<List<ListarAvaliacaoHabilidadeDto>> BuscarPorCandidato(int candidatoId)
        {
            if (candidatoId <= 0)
                throw new DomainException("O ID do candidato deve ser maior que zero.");

            var avaliacoes = await _repository.BuscarPorCandidato(candidatoId);
            return avaliacoes.Select(ConverterParaDto).ToList();
        }

        public async Task<ListarAvaliacaoHabilidadeDto> Adicionar(AdicionarAvaliacaoHabilidadeDto dto)
        {
            if (dto == null)
                throw new DomainException("Os dados da avaliação de habilidade são obrigatórios.");

            ValidarChave(dto.CandidatoId, dto.EntrevistaId, dto.HabilidadeId);
            ValidarNivel(dto.NivelAvaliado);

            var avaliacaoExistente = await _repository.BuscarPorId(
                dto.CandidatoId, dto.EntrevistaId, dto.HabilidadeId);

            if (avaliacaoExistente != null)
                throw new DomainException("Já existe uma avaliação para esse candidato, entrevista e habilidade.");

            var avaliacao = new AvaliacaoHabilidade
            {
                CandidatoId = dto.CandidatoId,
                EntrevistaId = dto.EntrevistaId,
                HabilidadeId = dto.HabilidadeId,
                NivelAvaliado = dto.NivelAvaliado
            };

            await _repository.Adicionar(avaliacao);

            var avaliacaoComRelacionamentos = await _repository.BuscarPorId(
                dto.CandidatoId, dto.EntrevistaId, dto.HabilidadeId);

            if (avaliacaoComRelacionamentos == null)
                throw new InvalidOperationException("A avaliação foi cadastrada, mas não foi possível recuperá-la.");

            return ConverterParaDto(avaliacaoComRelacionamentos);
        }

        public async Task<ListarAvaliacaoHabilidadeDto> Atualizar(
            int candidatoId, int entrevistaId, int habilidadeId,
            AtualizarAvaliacaoHabilidadeDto dto)
        {
            ValidarChave(candidatoId, entrevistaId, habilidadeId);

            if (dto == null)
                throw new DomainException("Os dados para atualização são obrigatórios.");

            ValidarNivel(dto.NivelAvaliado);

            var avaliacaoAtualizada = await _repository.Atualizar(
                candidatoId, entrevistaId, habilidadeId, dto.NivelAvaliado);

            if (avaliacaoAtualizada == null)
                throw new KeyNotFoundException("Avaliação de habilidade não encontrada.");

            return ConverterParaDto(avaliacaoAtualizada);
        }

        public async Task Deletar(int candidatoId, int entrevistaId, int habilidadeId)
        {
            ValidarChave(candidatoId, entrevistaId, habilidadeId);
            await _repository.Deletar(candidatoId, entrevistaId, habilidadeId);
        }
    }
}

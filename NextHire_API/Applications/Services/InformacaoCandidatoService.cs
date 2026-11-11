using System.Text.Json;
using NextHire_API.Domains;
using NextHire_API.DTOs.InformacaoCandidatoDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class InformacaoCandidatoService
    {
        private readonly IInformacaoCandidatoRepository _repository;

        public InformacaoCandidatoService(IInformacaoCandidatoRepository repository)
        {
            _repository = repository;
        }

        private static ListarInformacaoCandidatoDto ConverterParaDto(InformacaoCandidato informacao)
        {
            return new ListarInformacaoCandidatoDto
            {
                InfoCandidatoId = informacao.InfoCandidatoId,
                CandidatoId = informacao.CandidatoId,
                DataNascimento = informacao.DataNascimento,
                FormacaoAcademica = ConverterParaJson(informacao.FormacaoAcademica),
                Idioma = ConverterParaJson(informacao.Idioma),
                Resumo = informacao.Resumo,
                Telefone = informacao.Telefone,
                SituacaoProfissional = informacao.SituacaoProfissional
            };
        }

        private static JsonElement ConverterParaJson(string valor)
        {
            using JsonDocument documento = JsonDocument.Parse(valor);
            return documento.RootElement.Clone();
        }

        private static string ConverterJsonParaTexto(JsonElement valor, string nomeCampo)
        {
            if (valor.ValueKind == JsonValueKind.Undefined || valor.ValueKind == JsonValueKind.Null)
                throw new DomainException($"O campo {nomeCampo} é obrigatório e deve conter um JSON válido.");

            return valor.GetRawText();
        }

        private static void ValidarDados(
            DateOnly dataNascimento,
            JsonElement formacaoAcademica,
            JsonElement idioma,
            string resumo,
            string telefone,
            SituacaoProfissionalEnum situacaoProfissional)
        {
            if (dataNascimento == default || dataNascimento > DateOnly.FromDateTime(DateTime.Today))
                throw new DomainException("A data de nascimento é inválida.");

            _ = ConverterJsonParaTexto(formacaoAcademica, "FormacaoAcademica");
            _ = ConverterJsonParaTexto(idioma, "Idioma");

            if (string.IsNullOrWhiteSpace(resumo))
                throw new DomainException("O resumo não pode ser nulo ou vazio.");

            if (string.IsNullOrWhiteSpace(telefone))
                throw new DomainException("O telefone não pode ser nulo ou vazio.");

            if (!Enum.IsDefined(typeof(SituacaoProfissionalEnum), situacaoProfissional))
                throw new DomainException("A situação profissional informada é inválida.");
        }

        public List<ListarInformacaoCandidatoDto> Listar()
        {
            return _repository.Listar().Select(ConverterParaDto).ToList();
        }

        public ListarInformacaoCandidatoDto BuscarPorId(int id)
        {
            if (id <= 0)
                throw new DomainException("O ID da informação do candidato deve ser maior que zero.");

            InformacaoCandidato informacao = _repository.BuscarPorId(id);
            if (informacao == null)
                throw new KeyNotFoundException("Informações do candidato não encontradas.");

            return ConverterParaDto(informacao);
        }

        public ListarInformacaoCandidatoDto BuscarPorCandidato(int candidatoId)
        {
            if (candidatoId <= 0)
                throw new DomainException("O ID do candidato deve ser maior que zero.");

            InformacaoCandidato informacao = _repository.BuscarPorCandidato(candidatoId);
            if (informacao == null)
                throw new KeyNotFoundException("Não foram encontradas informações para esse candidato.");

            return ConverterParaDto(informacao);
        }

        public ListarInformacaoCandidatoDto Adicionar(AdicionarInformacaoCandidatoDto dto)
        {
            if (dto == null)
                throw new DomainException("Os dados da informação do candidato são obrigatórios.");

            if (dto.CandidatoId <= 0)
                throw new DomainException("O ID do candidato deve ser maior que zero.");

            ValidarDados(
                dto.DataNascimento,
                dto.FormacaoAcademica,
                dto.Idioma,
                dto.Resumo,
                dto.Telefone,
                dto.SituacaoProfissional);

            var informacao = new InformacaoCandidato
            {
                CandidatoId = dto.CandidatoId,
                DataNascimento = dto.DataNascimento,
                FormacaoAcademica = ConverterJsonParaTexto(dto.FormacaoAcademica, "FormacaoAcademica"),
                Idioma = ConverterJsonParaTexto(dto.Idioma, "Idioma"),
                Resumo = dto.Resumo.Trim(),
                Telefone = dto.Telefone.Trim(),
                SituacaoProfissional = dto.SituacaoProfissional
            };

            _repository.Adicionar(informacao);
            return ConverterParaDto(informacao);
        }

        public ListarInformacaoCandidatoDto Atualizar(int id, AtualizarInformacaoCandidatoDto dto)
        {
            if (id <= 0)
                throw new DomainException("O ID da informação do candidato deve ser maior que zero.");

            if (dto == null)
                throw new DomainException("Os dados para atualização são obrigatórios.");

            InformacaoCandidato informacao = _repository.BuscarPorId(id);
            if (informacao == null)
                throw new KeyNotFoundException("Informações do candidato não encontradas.");

            ValidarDados(
                dto.DataNascimento,
                dto.FormacaoAcademica,
                dto.Idioma,
                dto.Resumo,
                dto.Telefone,
                dto.SituacaoProfissional);

            informacao.DataNascimento = dto.DataNascimento;
            informacao.FormacaoAcademica = ConverterJsonParaTexto(dto.FormacaoAcademica, "FormacaoAcademica");
            informacao.Idioma = ConverterJsonParaTexto(dto.Idioma, "Idioma");
            informacao.Resumo = dto.Resumo.Trim();
            informacao.Telefone = dto.Telefone.Trim();
            informacao.SituacaoProfissional = dto.SituacaoProfissional;

            _repository.Atualizar(id, informacao);
            return ConverterParaDto(informacao);
        }

        public void Deletar(int id)
        {
            if (id <= 0)
                throw new DomainException("O ID da informação do candidato deve ser maior que zero.");

            InformacaoCandidato informacao = _repository.BuscarPorId(id);
            if (informacao == null)
                throw new KeyNotFoundException("Informações do candidato não encontradas.");

            _repository.Deletar(id);
        }
    }
}

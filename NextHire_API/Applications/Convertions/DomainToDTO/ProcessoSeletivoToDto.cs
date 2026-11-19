using NextHire_API.Domains;
using NextHire_API.DTOs.ProcessoSeletivoDTO;

namespace NextHire_API.Convertions.DomainToDTO
{
    public static class ProcessoSeletivoToDto
    {
        public static ListarProcessoSeletivoDto ConvertProcessoSeletivoToDto(ProcessoSeletivo processo)
        {
            return new ListarProcessoSeletivoDto
            {
                ProcessoSeletivoId = processo.ProcessoSeletivoId,
                TituloProcesso = processo.TituloProcesso,
                DataAbertura = processo.DataAbertura,
                DataFechamento = processo.DataFechamento,
                EmpresaId = processo.Vaga.EmpresaId,
                NomeEmpresa = processo.Vaga.Empresa.Nome,
                VagaId = processo.VagaId,
                NomeVaga = processo.Vaga.Titulo,
                StatusProcessoSeletivo = processo.StatusProcessoSeletivo == true ? "Ativo" : "Inativo"
            };
        }

        public static ProcessoSeletivo AdicionarProcessoSeletivoToDomain(AdicionarProcessoSeletivoDto processoDto)
        {
            return new ProcessoSeletivo
            {
                TituloProcesso = processoDto.TituloProcesso,
                VagaId = processoDto.VagaId,
                DataFechamento = processoDto.DataFechamento
            };
        }

        public static ProcessoSeletivo AtualizarProcessoSeletivoToDomain(ProcessoSeletivo processoSeletivo, string? tituloProcesso, DateOnly? dataFechamento)
        {
            if (!string.IsNullOrWhiteSpace(tituloProcesso))
                processoSeletivo.TituloProcesso = tituloProcesso;

            if (dataFechamento != null)
                processoSeletivo.DataFechamento = dataFechamento.Value;

            return processoSeletivo;
        }
    }
}
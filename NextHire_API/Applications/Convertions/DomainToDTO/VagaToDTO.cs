using System.Text.Json;
using NextHire_API.Domains;
using NextHire_API.DTOs.VagaDTO;

namespace NextHire_API.Applications.Convertions
{
    public class VagaToDTO
    {
        public static ListarVagaDto VagaToDto(Vaga vaga)
        {
            return new ListarVagaDto
            {
                VagaId = vaga.VagaId,
                NomeAnalista = vaga.AnalistaRhNavigation?.Nome ?? "Analista não encontrado",
                AnalistaRh = vaga.AnalistaRh,
                DataAbertura = vaga.DataAbertura,
                DataFechamento = vaga.DataFechamento,
                Descricao = vaga.Descricao,
                NomeEmpresa = vaga.Empresa?.Nome ?? "Empresa não encontrada",
                EmpresaId = vaga.EmpresaId,
                NomeSetor = vaga.Setor.Nome.ToUpper(),
                SetorId = vaga.SetorId,
                //? Para trabalhar com jsonb com o linq do EF Core
                IdiomasDesejados = LerIdiomas(vaga.IdiomasDesejados),
                StatusVaga = vaga.StatusVaga,
                NivelPrioridade = vaga.NivelPrioridadeVaga.ToString().ToLowerInvariant(),
                Remuneracao = vaga.Remuneracao,
                TipoVaga = vaga.TipoVaga.ToString(),
                TempoExperienciaDesejado = vaga.TempoExperienciaDesejado,
                Titulo = vaga.Titulo
            };
        }

        public static Vaga AtualizarVagaDtoToDomain(AtualizarVagaDto vaga)
        {
            return new Vaga
            {
                AnalistaRh = vaga.AnalistaRh,
                SetorId = vaga.SetorOcupacaoId,
                DataFechamento = vaga.DataFechamento,
                Descricao = vaga.Descricao,
                EmpresaId = vaga.EmpresaId,
                IdiomasDesejados = JsonSerializer.Serialize(vaga.IdiomasDesejados),
                Remuneracao = vaga.Remuneracao ?? 0,
                NivelPrioridadeVaga = ConverterNivelPrioridade(vaga.NivelPrioridade),
                TipoVaga = ConverterTipoVaga(vaga.TipoVaga ?? string.Empty),
                TempoExperienciaDesejado = vaga.TempoExperienciaDesejado,
                Titulo = vaga.Titulo
            };
        }

        public static Vaga AdicionarVagaDtoToDomain(AdicionarVagaDto vaga)
        {
            return new Vaga
            {
                AnalistaRh = vaga.AnalistaId,
                DataFechamento = vaga.DataFechamento,
                Descricao = vaga.Descricao,
                EmpresaId = vaga.EmpresaId,
                IdiomasDesejados = JsonSerializer.Serialize(vaga.IdiomasDesejados),
                Remuneracao = vaga.Remuneracao,
                NivelPrioridadeVaga = ConverterNivelPrioridade(vaga.NivelPrioridade),
                TipoVaga = ConverterTipoVaga(vaga.TipoVaga),
                TempoExperienciaDesejado = vaga.TempoExperienciaDesejado,
                StatusVaga = true,
                Titulo = vaga.Titulo,
                SetorId = vaga.SetorOcupacaoId
            };
        }

        /// <summary>Converte a string técnica do nível de prioridade para o enum do PostgreSQL.</summary>
        public static NivelPrioridadeEnum ConverterNivelPrioridade(string valor)
            => valor.Trim().ToLowerInvariant() switch
            {
                "alta" => NivelPrioridadeEnum.Alta,
                "media" => NivelPrioridadeEnum.Media,
                "baixa" => NivelPrioridadeEnum.Baixa,
                _ => throw new Exceptions.DomainException(
                    $"nivel_prioridade inválido: {valor}. Valores aceitos: alta, media, baixa.")
            };

        /// <summary>Converte a string técnica do tipo de vaga para o enum do PostgreSQL.</summary>
        public static TipoVagaEnum ConverterTipoVaga(string valor)
            => valor.Trim().ToLowerInvariant() switch
            {
                "nova_funcao" => TipoVagaEnum.NovaFuncao,
                "substituicao" => TipoVagaEnum.Substituicao,
                "expansao_equipe" => TipoVagaEnum.ExpansaoEquipe,
                _ => throw new Exceptions.DomainException(
                    $"tipo_vaga inválido: {valor}. Valores aceitos: nova_funcao, substituicao, expansao_equipe.")
            };


        //? Função para ler a string jsonb e retornar uma lista de strings
        //? Sem ela, o json vem porem com a sequencia de caracteres \u0022, que é o equivalente a aspas duplas
        //? e não é possível ler a lista de idiomas
        private static List<string> LerIdiomas(string json)
        {
            try
            {
                var lista = JsonSerializer.Deserialize<List<string>>(json);
                if (lista != null) return lista;
            }
            catch (JsonException) { }

            //? caso esteja double-encoded: desembrulha a string e tenta de novo
            try
            {
                var interno = JsonSerializer.Deserialize<string>(json);
                if (interno != null)
                    return JsonSerializer.Deserialize<List<string>>(interno) ?? new();
            }
            catch (JsonException) { }

            return new();
        }
    }
}
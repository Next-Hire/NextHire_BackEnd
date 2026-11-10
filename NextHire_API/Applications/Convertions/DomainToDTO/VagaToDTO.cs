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
                NomeAnalista = vaga.AnalistaRhNavigation.Nome ?? "Analista não encontrado",
                AnalistaRh = vaga.AnalistaRh,
                DataAbertura = vaga.DataAbertura,
                DataFechamento = vaga.DataFechamento,
                Descricao = vaga.Descricao,
                NomeEmpresa = vaga.Empresa.Nome,
                EmpresaId = vaga.EmpresaId,
                //? Para trabalhar com jsonb com o linq do EF Core
                IdiomasDesejados = LerIdiomas(vaga.IdiomasDesejados),
                StatusVaga = vaga.StatusVaga,
                TempoExperienciaDesejado = vaga.TempoExperienciaDesejado,
                Titulo = vaga.Titulo
            };
        }

        public static Vaga AtualizarVagaDtoToDomain(AtualizarVagaDto vaga)
        {
            return new Vaga
            {
                AnalistaRh = vaga.AnalistaRh,
                DataFechamento = vaga.DataFechamento,
                Descricao = vaga.Descricao,
                EmpresaId = vaga.EmpresaId,
                IdiomasDesejados = JsonSerializer.Serialize(vaga.IdiomasDesejados),
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
                TempoExperienciaDesejado = vaga.TempoExperienciaDesejado,
                StatusVaga = true,
                Titulo = vaga.Titulo
            };
        }


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
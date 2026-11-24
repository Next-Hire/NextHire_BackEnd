using NextHire_API.Applications.Convertions.DomainToDTO;
using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoQuestaoDTO;
using NextHire_API.DTOs.CandidatoVagaDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;
using System.Text.Json;
using NextHireAPI.Convertions.DomainToDTO;
using NextHireAPI.Domains;

namespace NextHire_API.Applications.Convertions.DomainToDto
{
    public static class CurriculoVagaToDTO
    {
        public static ListarCurriculoVagaDto CurriculoVagaToDto(CurriculoVaga curriculoVaga)
        {
            double ObterNotaMedia()
            {
                List<CandidatoQuestao> questoesCandidato = curriculoVaga.Curriculo.Candidato.CandidatoQuestao?.ToList() ?? new List<CandidatoQuestao>();
                List<LerCandidatoQuestaoGabaritoDTO> candidatoQuestaoDto = questoesCandidato.Select(c => CandidatoQuestaoToDto.LerComGabarito(c)).ToList();

                int totalQuestoes = candidatoQuestaoDto.Count;
                List<bool?> questoesCertas = candidatoQuestaoDto.Select(c => c.acertou).ToList();

                if (totalQuestoes == 0)
                    return 0; // evita divisão por zero quando o candidato não respondeu nenhuma questão

                return questoesCertas.Count(q => q == true) / (double)totalQuestoes * 100;
            }

            double ObterCompatibilidade(List<Habilidade> habilidadesVaga, List<Habilidade> habilidadesCandidato)
            {
                double notaCompatibilidade = 0;

                // Lógica para calcular a compatibilidade

                return notaCompatibilidade; // Retorna a nota de compatibilidade original por enquanto
            }

            if (curriculoVaga == null)
                throw new DomainException("O objeto CurriculoVaga não pode ser nulo.");

            return new ListarCurriculoVagaDto
            {
                NomeCandidato = curriculoVaga.Curriculo.Candidato.Nome,
                CandidatoId = curriculoVaga.Curriculo.CandidatoId,
                NotaMediaQuestoes = ObterNotaMedia(),
                NomeVaga = curriculoVaga.Vaga.Titulo,
                NotaCompatibilidadeVaga = ObterCompatibilidade(
                    curriculoVaga.Vaga.Habilidade.ToList(),
                    curriculoVaga.Curriculo.Candidato.HabilidadeCandidato.Select(hc => hc.Habilidade).ToList()),
                VagaId = curriculoVaga.VagaId,
                DataHoraInscricao = curriculoVaga.DataHoraInscricao,
                CurriculoConvertido = SerializarCurriculo(curriculoVaga.Curriculo.CurriculoConvertido)
            };
        }

        /// <summary>
        /// Normaliza o conteúdo do currículo que vem do banco como coluna jsonb.
        /// O PostgreSQL entrega o jsonb como string (podendo chegar double-encoded,
        /// com aspas escapadas \u0022). Aqui desserializamos e re-serializamos com
        /// JsonSerializer.Serialize() para devolver um JSON limpo ao cliente.
        /// </summary>
        private static string? SerializarCurriculo(string? curriculoJsonb)
        {
            if (string.IsNullOrWhiteSpace(curriculoJsonb))
                return null;

            try
            {
                JsonElement conteudo = JsonSerializer.Deserialize<JsonElement>(curriculoJsonb);
                return JsonSerializer.Serialize(conteudo);
            }
            catch (JsonException)
            {
                // Não é JSON válido — devolve o original sem quebrar a resposta
                return curriculoJsonb;
            }
        }
    }
}

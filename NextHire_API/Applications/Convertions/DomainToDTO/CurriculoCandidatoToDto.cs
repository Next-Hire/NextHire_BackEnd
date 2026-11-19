using Microsoft.OpenApi;
using NextHire_API.Domains;
using NextHire_API.DTOs.CurriculoAWS;
using System.Text.Json;

namespace NextHire_API.Applications.Convertions.DomainToDto
{
    public class CurriculoCandidatoToDto
    {

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

       private static CurriculoDTO? desserializarResposta(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<CurriculoDTO>(json, _jsonOptions);
        }

        private static string SerializarCurriculo(CurriculoDTO dto)
            => JsonSerializer.Serialize(dto, _jsonOptions);

        public static LerCurriculoCandidatoDTO LerCurriculoToDTO(CurriculoCandidato cucCandidato)
            => new LerCurriculoCandidatoDTO
            {
                curriculoCandidatoId = cucCandidato.CandidatoId,
                Usuario =
                {
                    candidatoId = cucCandidato.CandidatoId,
                    nome = cucCandidato.Candidato.Nome
                },
                CurriculoConvertido = desserializarResposta(cucCandidato.CurriculoConvertido)
                
            };


        public static CriarCurriculoDTO criarCurriculo(CurriculoCandidato curriculoCandidato)
            => new CriarCurriculoDTO
            {
                candidatoId = curriculoCandidato.CandidatoId,
                nomeCandidato = curriculoCandidato.Candidato?.Nome ?? string.Empty,
                curriculoConvertido = desserializarResposta(curriculoCandidato.CurriculoConvertido)
            };


    }
}

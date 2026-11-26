using System.Text.Json;
using NextHire_API.Domains;
using NextHire_API.DTOs.EntrevistaDTO;

namespace NextHire_API.Applications.Convertions.DomainToDto
{
    public class EntrevistaToDTO
    {
        public static LerEntrevistaDTO LerDTO(Entrevista entrevista)
        {
            DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);

            return new LerEntrevistaDTO
            {
                entrevistaId = entrevista.EntrevistaId,
                tituloEntrevista = entrevista.NomeEntrevista,
                dataInicio = entrevista.HorarioInicio,
                dataEncerra = entrevista.HorarioFim,
                statusEntrevista = entrevista.StatusEntrevista,
                link = entrevista.Link,
                ProcessoSeletivoId = entrevista.ProcessoSeletivoId,
                ListaCandidatos = entrevista.Candidato?.Select(varAux =>
                {
                    var info = varAux.InformacaoCandidato?.FirstOrDefault();

                    int idade = 0;
                    if (info != null)
                    {
                        idade = hoje.Year - info.DataNascimento.Year;
                        if (info.DataNascimento > hoje.AddYears(-idade))
                            idade--;
                    }

                    return new CandidatoDTO
                    {
                        candidatoId = varAux.UsuarioId,
                        nomeCandidato = varAux.Nome,
                        email = varAux.Email,
                        telefone = info?.Telefone ?? string.Empty,
                        idade = idade,
                        descricao = info?.Resumo ?? string.Empty,
                        localizacao = info?.Endereco ?? string.Empty, // coluna nullable em informacao_candidato
                        contatos = ObterContatos(info?.Contato)
                    };
                }).ToList() ?? new List<CandidatoDTO>()
            };
        }

        //? contato é um jsonb nullable: aceita ["email", "linkedin"] ou [{"tipo": "...", "valor": "..."}]
        private static List<string> ObterContatos(string? contatoJson)
        {
            if (string.IsNullOrWhiteSpace(contatoJson))
                return new List<string>();

            try
            {
                using JsonDocument documento = JsonDocument.Parse(contatoJson);
                JsonElement raiz = documento.RootElement;

                if (raiz.ValueKind != JsonValueKind.Array)
                    return new List<string>();

                var contatos = new List<string>();

                foreach (JsonElement item in raiz.EnumerateArray())
                {
                    switch (item.ValueKind)
                    {
                        case JsonValueKind.String:
                            AdicionarSeValido(contatos, item.GetString());
                            break;

                        case JsonValueKind.Number:
                            contatos.Add(item.GetRawText());
                            break;

                        case JsonValueKind.Object:
                            string? texto = null;
                            foreach (string chave in new[] { "valor", "value", "descricao", "contato" })
                            {
                                if (item.TryGetProperty(chave, out JsonElement valor) && valor.ValueKind == JsonValueKind.String)
                                {
                                    texto = valor.GetString();
                                    break;
                                }
                            }

                            if (texto != null)
                            {
                                AdicionarSeValido(contatos, texto);
                                break;
                            }

                            foreach (JsonProperty propriedade in item.EnumerateObject())
                                if (propriedade.Value.ValueKind == JsonValueKind.String)
                                    AdicionarSeValido(contatos, propriedade.Value.GetString());
                            break;
                    }
                }

                return contatos;
            }
            catch (JsonException)
            {
                return new List<string>();
            }
        }

        private static void AdicionarSeValido(List<string> contatos, string? valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                contatos.Add(valor.Trim());
        }
    }
}
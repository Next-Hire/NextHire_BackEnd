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
                ProcessoSeletivoId = entrevista.ProcessoSeletivoId,
                ListaCandidatos = entrevista.Candidato.Select(varAux =>
                {
                    var info = varAux.InformacaoCandidato.FirstOrDefault();

                    int idade = 0;
                    if (idade != null || idade == 0)
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
                        telefone = info.Telefone,
                        idade = idade,
                        descricao = info?.Resumo,
                        localizacao = string.Empty, // adicionar localizacao
                        contatos = new List<string>() // adicionar contatos no banco tambem
                    };
                }).ToList() ?? new List<CandidatoDTO>()
            }; 
        
        }
    }
}

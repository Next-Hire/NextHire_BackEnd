using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoProcessoSeletivoDTO;

namespace NextHire_API.Applications.Convertions.DomainToDTO;

public class CandidatoProcessoSeletivoToDTO
{
    public static LerCandidatoProcessoDTO LerDTO(CandidatoProcessoSeletivo candProcesso)
        => new LerCandidatoProcessoDTO
        {
            processoSeletivoId = candProcesso.ProcessoSeletivoId,
            nomeProcesso = candProcesso.ProcessoSeletivo?.TituloProcesso ??  string.Empty,
            candidatoId = candProcesso.CandidatoId,
            nomeCandidato = candProcesso.Candidato?.Nome ?? string.Empty,
            StatusRelacao =  candProcesso.StatusRelacao,
        };
}
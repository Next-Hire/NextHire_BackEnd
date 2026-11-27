namespace NextHire_API.DTOs.EntrevistaDTO
{
    public class LerEntrevistaDTO
    {
        public int entrevistaId { get; set; }


        public string tituloEntrevista { get; set; } = string.Empty!;

        public DateTime? dataInicio { get; set; }

        public DateTime? dataEncerra { get; set; }

        public bool? statusEntrevista { get; set; }

        public string? link { get; set; }

        public int ProcessoSeletivoId { get; set; }

        public List<CandidatoDTO> ListaCandidatos { get; set; } = new List<CandidatoDTO>();


    }


    public class CandidatoDTO
    {
        public int candidatoId { get; set; }
        public string nomeCandidato { get; set; } = string.Empty!;
        public string email { get; set; } = string.Empty!;
        public string telefone { get; set; } = string.Empty!;
        public int idade { get; set; }

        public string localizacao { get; set; } = string.Empty!;
        public string descricao { get; set; }   = string.Empty!;

        public List<string> contatos { get; set; } = new List<string>();

    }
}

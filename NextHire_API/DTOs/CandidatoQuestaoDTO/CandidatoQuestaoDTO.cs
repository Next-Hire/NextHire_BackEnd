namespace NextHire_API.DTOs.CandidatoQuestaoDTO
{
    public class RespostaCandidatoQuestaoDTO
    {
        public int QuestaoId { get; set; }

        public string tituloQuestao { get; set; } = null!;

       

        public string alternativaEscolhida { get; set; } = null!;

        public string tipoQuestaoEnum { get; set; } = null!;

        public int AlternativaEscolhidaId { get; set; }

        public bool estaCerta { get; set; }

    }



}

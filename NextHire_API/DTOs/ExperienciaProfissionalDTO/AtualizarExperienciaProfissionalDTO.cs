using NextHire_API.Domains;

namespace NextHire_API.DTOs.ExperienciaProfissionalDTO
{
    public class AtualizarExperienciaProfissionalDTO
    {
        public DateTime data_entrada { get; set; }

        public DateTime? data_saida { get; set; }

        public bool? atualmente_trabalho { get; set; }
    }
}

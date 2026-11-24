using System.Text.Json;
using NextHire_API.Domains;

namespace NextHire_API.DTOs.InformacaoCandidatoDTO
{
    public class ListarInformacaoCandidatoDto
    {
        public int InfoCandidatoId { get; set; }
        public int CandidatoId { get; set; }
        public DateOnly DataNascimento { get; set; }
        public JsonElement FormacaoAcademica { get; set; }
        public JsonElement Idioma { get; set; }
        public string Resumo { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public SituacaoProfissionalEnum SituacaoProfissional { get; set; }
    }
}

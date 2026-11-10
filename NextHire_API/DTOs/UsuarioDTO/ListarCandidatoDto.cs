using NextHire_API.Domains;

namespace NextHireAPI.Domains
{
    public partial class ListarCandidatoDto
    {
        public int CandidatoId { get; set; }

        public string Email { get; set; } = null!;

        public string Nome { get; set; } = null!;

        public bool? PrimeiroAcesso { get; set; }

        public bool? StatusUsuario { get; set; }

        public TipoUsuarioEnum TipoUsuario { get; set; }

        public virtual ICollection<CurriculoCandidato> CurriculoCandidato { get; set; } = new List<CurriculoCandidato>();

        public virtual ICollection<ExperienciaProfissional> ExperienciaProfissional { get; set; } = new List<ExperienciaProfissional>();

        public virtual ICollection<HabilidadeCandidato> HabilidadeCandidato { get; set; } = new List<HabilidadeCandidato>();

        public virtual ICollection<InformacaoCandidato> InformacaoCandidato { get; set; } = new List<InformacaoCandidato>();
    }
}

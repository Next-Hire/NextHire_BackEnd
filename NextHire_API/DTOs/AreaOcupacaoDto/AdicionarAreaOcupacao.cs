using NextHire_API.Domains;

namespace NextHire_API.DTOs.AreaOcupacaoDto
{
    public class AdicionarAreaOcupacao
    {
        public string Nome { get; set; } = null!;
        public virtual ICollection<SetorOcupacao> SetorOcupacao { get; set; } = new List<SetorOcupacao>();
        public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
    }
}

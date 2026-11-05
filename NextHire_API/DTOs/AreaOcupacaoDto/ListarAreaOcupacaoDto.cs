using NextHire_API.Domains;

namespace NextHire_API.DTOs.AreaOcupacaoDto
{
    public class ListarAreaOcupacaoDto
    {
        public int AreaOcupacaoId { get; set; }
        public string Nome { get; set; } = null!;
        public virtual ICollection<SetorOcupacao> SetorOcupacao { get; set; } = new List<SetorOcupacao>();
        public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
    }
}

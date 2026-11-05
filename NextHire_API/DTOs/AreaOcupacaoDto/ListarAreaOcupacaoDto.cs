using NextHire_API.Domains;

namespace NextHire_API.DTOs.AreaOcupacaoDto
{
    public class ListarAreaOcupacaoDto
    {
        public int area_ocupacao_id { get; set; }
        public string nome { get; set; } = null!;
        public virtual ICollection<SetorOcupacao> SetorOcupacao { get; set; } = new List<SetorOcupacao>();
        public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
    }
}

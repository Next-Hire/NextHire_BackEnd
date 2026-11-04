using NextHire_API.Domains;

namespace NextHire_API.DTOs.AreaOcupacaoDto
{
    public class AdicionarAreaOcupacao
    {
        public string nome { get; set; } = null!;
        public virtual ICollection<setor_ocupacao> setor_ocupacao { get; set; } = new List<setor_ocupacao>();
        public virtual ICollection<usuario> usuario { get; set; } = new List<usuario>();
    }
}

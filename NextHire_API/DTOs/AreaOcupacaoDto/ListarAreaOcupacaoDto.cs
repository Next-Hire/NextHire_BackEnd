using NextHire_API.Domains;

namespace NextHire_API.DTOs.AreaOcupacaoDto
{
    public class ListarAreaOcupacaoDto
    {
        public int area_ocupacao_id { get; set; }
        public string nome { get; set; } = null!;
        public virtual ICollection<setor_ocupacao> setor_ocupacao { get; set; } = new List<setor_ocupacao>();
        public virtual ICollection<usuario> usuario { get; set; } = new List<usuario>();
    }
}

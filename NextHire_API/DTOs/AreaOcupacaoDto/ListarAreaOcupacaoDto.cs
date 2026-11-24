using NextHire_API.Domains;

namespace NextHire_API.DTOs.AreaOcupacaoDto
{
    public class ListarAreaOcupacaoDto
    {
        public int AreaOcupacaoId { get; set; }
        public string Nome { get; set; } = null!;
    }
}

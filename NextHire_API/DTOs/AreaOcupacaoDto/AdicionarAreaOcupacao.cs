using NextHire_API.Domains;

namespace NextHire_API.DTOs.AreaOcupacaoDto
{
    public class AdicionarAreaOcupacao
    {
        public string Nome { get; set; } = null!;
        public int SetorId { get; set; }
        public int UsuarioId { get; set; }
    }
}

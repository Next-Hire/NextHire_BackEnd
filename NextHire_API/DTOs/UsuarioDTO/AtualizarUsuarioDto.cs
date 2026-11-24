using NextHire_API.Domains;

namespace NextHireAPI.Domains
{
    public partial class AtualizarUsuarioDto
    {
        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;
        
    }
}
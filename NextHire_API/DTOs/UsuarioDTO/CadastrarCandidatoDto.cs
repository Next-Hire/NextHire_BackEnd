using System.Net.Mail;

namespace NextHireAPI.DTOs.UsuarioDto
{
    public partial class CadastrarCandidatoDto
    {
        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string senha { get; set;}
    }
}
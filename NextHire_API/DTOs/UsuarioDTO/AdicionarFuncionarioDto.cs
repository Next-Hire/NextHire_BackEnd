using System.Net.Mail;
using NextHire_API.Domains;

namespace NextHireAPI.Domains
{
    public partial class AdicionarFuncionarioDto
    {
        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public TipoUsuarioEnum Cargo { get; set; }
    }
}
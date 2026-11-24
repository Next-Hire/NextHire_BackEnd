using NextHire_API.Domains;

namespace NextHireAPI.Domains
{

    //? Esta DTO é somente para testes.
    public partial class ListarUsuariosDto
    {
        public int UsuarioId { get; set; }

        public string Email { get; set; } = null!;

        public string Nome { get; set; } = null!;

        public bool? PrimeiroAcesso { get; set; }

        public bool? StatusUsuario { get; set; }

        public TipoUsuarioEnum TipoUsuario { get; set; }
    }
}

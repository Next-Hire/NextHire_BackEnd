using NextHire_API.Domains;

namespace NextHireAPI.Domains
{
    public partial class ListarFuncionarioDto
    {
        public int FuncionarioId { get; set; }

        public string Email { get; set; } = null!;

        public string Nome { get; set; } = null!;

        public int EmpresaId { get; set; }
        
        public string NomeEmpresa { get; set; } = null!;

        public bool? PrimeiroAcesso { get; set; }

        public bool? StatusUsuario { get; set; }

        public TipoUsuarioEnum TipoUsuario { get; set; }
    }
}
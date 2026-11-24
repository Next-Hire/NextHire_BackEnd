namespace NextHireApi.DTOs.UsuarioDTO
{
    public partial class AtualizarSenhaDto
    {
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string CodigoVerificacao { get; set; } = null!;
    }
}
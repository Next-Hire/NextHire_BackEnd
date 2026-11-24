namespace NextHireAPI.DTOs.Autenticacao;

public partial class TokenResponseDto
{
    public string Token { get; set; } = string.Empty!;
    public string TipoUsuario { get; set; } = string.Empty!;
}
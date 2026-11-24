using System.ComponentModel.DataAnnotations;

namespace NextHire_API.DTOs.EmpresaDTOs
{
    
public class CriarEmpresaDTO2
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string nome { get; set; } = string.Empty;
}


public class LerEmpresaDTO2
{
    public int EmpresaId { get; set; }
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string nome { get; set; } = string.Empty;
    public bool statusEmpresa { get; set; }
}

public class AtualizarEmpresaDTO2
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string nome { get; set; } = string.Empty;
    public bool statusEmpresa { get; set; }
}
}

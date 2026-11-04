using System.ComponentModel.DataAnnotations;

namespace NextHire_API.DTOs.EmpresaDTOs
{
    public class CriarEmpresaDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string nome { get; set; } = string.Empty;
    }


    public class LerEmpresaDTO
    {
        public int empresaId { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string nome { get; set; } = string.Empty;
    }
}

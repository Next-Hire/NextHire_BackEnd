using System.ComponentModel.DataAnnotations;

namespace NextHire_API.DTOs.EmpresaDTOs
{
    public class criarEmpresaDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;
    }

    public class lerEmpresaDTO
    {
        public int? empresaId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;
    }
}

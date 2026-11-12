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
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string nome { get; set; } = string.Empty;
        public bool statusEmpresa { get; set; }
    }

    public class AtualizarEmpresaDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string nome { get; set; } = string.Empty;
        public bool statusEmpresa { get; set; }
    }
    
    

    
}

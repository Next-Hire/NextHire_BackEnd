using System.ComponentModel.DataAnnotations;

namespace NextHire_API.DTOs.TipoEntrevistaDTO
{
    public class ListarTipoEntrevista
    {
        public int tipoEntrevistaId { get; set; }
        [Required(ErrorMessage = "Um nome é necessário")]
        public string nome { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Uma descrição é necessária")]
        public string descricao { get; set; } = string.Empty;


    }
}

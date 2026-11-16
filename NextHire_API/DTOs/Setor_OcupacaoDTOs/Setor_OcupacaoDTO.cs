using System.ComponentModel.DataAnnotations;
using NextHire_API.DTOs.EmpresaDTOs;

namespace NextHire_API.DTOs.Setor_OcupacaoDTOs
{
    public class CriarSetor_OcupacaoDTO
    {
        [Required(ErrorMessage = "O nome do setor é obrigatório.")]
        public string nomeSetor { get; set; } = string.Empty;

        public int areaId { get; set; }
        public string? nomeArea { get; set; }

        public int empresaId { get; set; }
        public string? nomeEmpresa { get; set; }

        public List<int>? gestoresIds { get; set; }
        public List<string>? nomeGestor { get; set; }
        
        public List<GestorSetor_OcupacaoDTO?> gestoresB { get; set; }
    }

    public class LerSetor_OcupacaoDTO
    {
        public int? setor_OcupacaoId { get; set; }
        [Required(ErrorMessage = "Nome do setor é obrigatório")]
        public string nomeSetor { get; set; } = string.Empty;
        
        
        public int? areaId { get; set; }
        [Required(ErrorMessage = "Nome da área é obrigatório")]
        public string nomeArea { get; set; } = string.Empty;
        
        public int? empresaId { get; set; }
        [Required(ErrorMessage = "Nome da empresa é obrigatório")]
        public string nomeEmpresa { get; set; } = string.Empty;
        
        public List<int> gestorIds { get; set; }

        [Required(ErrorMessage = "A lista de gestores é obrigatória")]
        [MinLength(1, ErrorMessage = "Cadastre ao menos um gestor para este setor")]
        public List<string> nomeGestor { get; set; } = new List<string> { string.Empty };
    }

    public class LerSetor_OcupacaoDTO2
    {
        public int? setor_OcupacaoId { get; set; }
        [Required(ErrorMessage = "Nome do setor é obrigatório")]
        public string nomeSetor { get; set; } = string.Empty;

        public LerEmpresaDTO Empresa { get; set; } = null!;
        public AreaSetor_OcupacaoDTO Area { get; set; } = null!;
        public List<GestorSetor_OcupacaoDTO> Gestores { get; set; } = new(); // lista vazia btw
    }

    public class AtualizarSetor_OcupacaoDTO
    {
        
        [Required(ErrorMessage = "Nome do setor é obrigatório")]
        public string nomeSetor { get; set; } = string.Empty;
        
        
        public int? areaId { get; set; }
        [Required(ErrorMessage = "Nome da área é obrigatório")]
        public string nomeArea { get; set; } = string.Empty;
        
        public int? empresaId { get; set; }
        [Required(ErrorMessage = "Nome da empresa é obrigatório")]
        public string nomeEmpresa { get; set; } = string.Empty;
        
        public List<int> gestorIds { get; set; }

        [Required(ErrorMessage = "A lista de gestores é obrigatória")]
        [MinLength(1, ErrorMessage = "Cadastre ao menos um gestor para este setor")]
        public List<string> nomeGestor { get; set; } = new List<string> { string.Empty };
    }


    public class AreaSetor_OcupacaoDTO
    {
        public int area_ocupacao_id { get; set; }
        public string nome { get; set; } = null!;
        public bool status { get; set; }
    }

    public class GestorSetor_OcupacaoDTO
    {
        public int? usuarioId { get; set; }
        public string nome { get; set; }
    }
    
    

    
}

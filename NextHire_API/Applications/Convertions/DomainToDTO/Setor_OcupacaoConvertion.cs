using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;
using NextHire_API.DTOs.Setor_OcupacaoDTOs;

namespace NextHire_API.Applications.Convertions.DomainToConversion
{
    public class Setor_OcupacaoConvertion
    {
        public static CriarSetor_OcupacaoDTO CriarSetorOcupacao(SetorOcupacao setorOcup) 
        {
            return new CriarSetor_OcupacaoDTO
            {
                nomeSetor = setorOcup.Nome,
                areaId = setorOcup.AreaOcupacaoId,
                empresaId = setorOcup.EmpresaId,
                gestoresB = setorOcup.Gestor?.Select(varAux => new GestorSetor_OcupacaoDTO
                {
                    usuarioId = varAux.UsuarioId,
                    nome = varAux.Nome
                    
                }).ToList() ?? new List<GestorSetor_OcupacaoDTO>()
            };
        }

        public static LerSetor_OcupacaoDTO listarSetoresOcupacoes(SetorOcupacao setorOcup)
        {
            return new LerSetor_OcupacaoDTO
            {
                nomeSetor = setorOcup.Nome,
                areaId = setorOcup.AreaOcupacaoId,
                nomeArea = setorOcup.AreaOcupacao.Nome ?? string.Empty,
                empresaId = setorOcup.EmpresaId,
                nomeEmpresa = setorOcup.Empresa.Nome ?? string.Empty,
                gestorIds = setorOcup.Gestor?.Select(varAux => varAux.UsuarioId).ToList() ?? new List<int>(),
                nomeGestor = setorOcup.Gestor?.Select(varAux => varAux.Nome).ToList() // Listando a colecao dos usuarios
            };
        }


        public static LerSetor_OcupacaoDTO2 lerSetoresOcupacoes(SetorOcupacao setorOcup)
        {
            return new LerSetor_OcupacaoDTO2
            {
                setor_OcupacaoId = setorOcup.SetorOcupacaoId,
                nomeSetor = setorOcup.Nome,
                Empresa = new LerEmpresaDTO
                {
                    EmpresaId = setorOcup.EmpresaId,
                    nome = setorOcup.Empresa.Nome
                    
                },
                
                Area = new AreaSetor_OcupacaoDTO
                {
                    area_ocupacao_id = setorOcup.AreaOcupacaoId,
                    nome = setorOcup.AreaOcupacao.Nome
                },
                
                Gestores = setorOcup.Gestor?.Select(varAux => new GestorSetor_OcupacaoDTO
                {
                    usuarioId = varAux.UsuarioId,
                    nome = varAux.Nome
                }).ToList() ?? new List<GestorSetor_OcupacaoDTO>()
            };
        }

    }
}

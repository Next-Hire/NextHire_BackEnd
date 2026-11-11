using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;

namespace NextHire_API.Applications.Convertions.DomainToConversion
{
    public class EmpresaConvertion
    {
        public static CriarEmpresaDTO criarEmpresaDTO(Empresa empresa)
        {
            return new CriarEmpresaDTO
            {
                nome = empresa.Nome,
            };
        }
        
        public static AtualizarEmpresaDTO atualizarEmpresaDTO(Empresa empresa)
        {
            return new AtualizarEmpresaDTO
            {
                nome = empresa.Nome,
                statusEmpresa = empresa.StatusEmpresa ?? true
            };
        }

        public static LerEmpresaDTO listarEmpresas(Empresa empresa)
        {
            return new LerEmpresaDTO
            {
                EmpresaId = empresa.EmpresaId,
                nome = empresa.Nome,
                statusEmpresa = empresa.StatusEmpresa ?? true

            };
        }

    }
}

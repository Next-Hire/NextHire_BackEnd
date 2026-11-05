using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;

namespace NextHire_API.Applications.Convertions.DomainToConversion
{
    public class EmpresaConvertion
    {
        public static CriarEmpresaDTO criarEmpresaDTO(empresa empresa)
        {
            return new CriarEmpresaDTO
            {
                nome = empresa.nome
            };
        }

        public static LerEmpresaDTO listarEmpresas(empresa empresa)
        {
            return new LerEmpresaDTO
            {
                empresaId = empresa.empresa_id,
                nome = empresa.nome
            };
        }

    }
}

using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;

namespace NextHire_API.Applications.Convertions.DomainToConversion
{
    public class EmpresaConvertion
    {
        public static criarEmpresaDTO criarEmpresaDTO(empresa empresa)
        {
            return new criarEmpresaDTO
            {
                Nome = empresa.nome
            };
        }

        public static lerEmpresaDTO listarEmpresas(empresa empresa)
        {
            return new lerEmpresaDTO
            {
                empresaId = empresa.empresa_id,
                Nome = empresa.nome
            };
        }

    }
}

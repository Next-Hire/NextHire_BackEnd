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
                nome = empresa.Nome
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

using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;

namespace NextHire_API.Applications.Convertions.DomainToDTO
{
    public static class EmpresaConversoes
    {
        public static LerEmpresaDTO LerEmpresa(empresa empresa)
        {
            return new LerEmpresaDTO
            {
                empresaId = empresa.empresa_id,
                nome = empresa.nome
                
            };
        }

        public static CriarEmpresaDTO CriarEmpresa(empresa empresa)
        {
            return new CriarEmpresaDTO
            {
                nome = empresa.nome,
            };
        }
    }
}

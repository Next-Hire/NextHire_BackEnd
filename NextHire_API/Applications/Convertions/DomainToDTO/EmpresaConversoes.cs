using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;

namespace NextHire_API.Applications.Convertions.DomainToDTO
{
    public static class EmpresaConversoes
    {
        public static LerEmpresaDTO LerEmpresa(Empresa Empresa)
        {
            return new LerEmpresaDTO
            {
                EmpresaId = Empresa.EmpresaId,
                nome = Empresa.Nome,
                statusEmpresa = Empresa.StatusEmpresa ?? true

            };
        }

        public static CriarEmpresaDTO CriarEmpresa(Empresa Empresa)
        {
            return new CriarEmpresaDTO
            {
                nome = Empresa.Nome
            };
        }
    }
}

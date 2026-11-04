using NextHire_API.Applications.Convertions.DomainToDTO;
using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class EmpresaService
    {
        private readonly IEmpresaRepository _rep;

        public EmpresaService(IEmpresaRepository rep) => _rep = rep;

        public async Task<IEnumerable<LerEmpresaDTO>> Listar()
        {
            IEnumerable<empresa?> empresaList = await _rep.Listar();
            IEnumerable<LerEmpresaDTO> LerDTO = empresaList.Select(varAux => EmpresaConversoes.LerEmpresa(varAux)).ToList();
            return LerDTO;
        }

    }
}

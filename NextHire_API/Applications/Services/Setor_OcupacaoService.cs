using NextHire_API.Applications.Convertions.DomainToConversion;
using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class Setor_OcupacaoService
    {
        private readonly ISetor_OcupacaoRepository _rep;

        public Setor_OcupacaoService(ISetor_OcupacaoRepository rep)
        {
            _rep = rep;
        }
        
        
 

    }
}

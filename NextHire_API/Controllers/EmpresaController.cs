using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.EmpresaDTOs;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly EmpresaService _rep;

        public EmpresaController(EmpresaService rep) => _rep = rep;

        [HttpGet]
        public async Task<IEnumerable<LerEmpresaDTO>> Listar()
        {
            try
            {
                IEnumerable<LerEmpresaDTO?> Query = await _rep.Listar();
                return Ok(Query);
            }

            catch (DomainException ex)
            {
                throw new DomainException("Empresas não encontradas");
            }
        }
    }
}

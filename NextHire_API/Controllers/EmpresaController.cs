using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {

        private readonly EmpresaService _service;

        public EmpresaController(EmpresaService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var empresaQuery = _service.ListagemTotal();
                return Ok(empresaQuery);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}

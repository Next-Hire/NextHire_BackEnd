using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.S3;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculoController : ControllerBase
    {
        private CurriculoService _service;

        public CurriculoController(CurriculoService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult> ObterCurriculo(string nomeObjeto)
        {
            try
            {
                string link = await _service.GerarLinkVisualizacaoCurriculo(nomeObjeto);
                return Ok(link);
            }

            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}

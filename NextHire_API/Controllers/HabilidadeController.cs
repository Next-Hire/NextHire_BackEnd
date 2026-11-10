using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.HabilidadeDto;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabilidadeController : ControllerBase
    {
        private readonly HabilidadesService _service;
        public HabilidadeController(HabilidadesService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<ListarHabilidadeDto> Listar()
        {
            try
            {
                var habilidade = _service.Listar();
                return Ok(habilidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ListarHabilidadeDto> ListarPorID(int id)
        {
            try
            {
                var habilidade = _service.ListarPorID(id);
                return Ok(habilidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar(AdicionarHabilidadeDto habilidade)
        {
            try
            {
               ListarHabilidadeDto habilidadedto = _service.Adicionar(habilidade);
                return StatusCode(201, habilidadedto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(int id, AdicionarHabilidadeDto habilidade)
        {
            try
            {
               ListarHabilidadeDto habilidadedto = _service.Atualizar(id, habilidade);
                return StatusCode(204, habilidadedto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Deletar(int id)
        {
            try
            {
                _service.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

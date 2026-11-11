using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.InformacaoCandidatoDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformacaoCandidatoController : ControllerBase
    {
        private readonly InformacaoCandidatoService _service;

        public InformacaoCandidatoController(InformacaoCandidatoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            try
            {
                return Ok(_service.Listar());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                return Ok(_service.BuscarPorId(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("candidato/{candidatoId:int}")]
        public IActionResult BuscarPorCandidato(int candidatoId)
        {
            try
            {
                return Ok(_service.BuscarPorCandidato(candidatoId));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] AdicionarInformacaoCandidatoDto dto)
        {
            try
            {
                var informacao = _service.Adicionar(dto);
                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = informacao.InfoCandidatoId },
                    informacao);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Atualizar(int id, [FromBody] AtualizarInformacaoCandidatoDto dto)
        {
            try
            {
                return Ok(_service.Atualizar(id, dto));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _service.Deletar(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

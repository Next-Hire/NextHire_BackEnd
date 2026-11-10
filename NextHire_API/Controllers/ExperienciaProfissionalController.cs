using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.ExperienciaProfissionalDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExperienciaProfissionalController : ControllerBase
    {
        private readonly ExperienciaProfissionalService _service;
        public ExperienciaProfissionalController(ExperienciaProfissionalService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ListarExperienciasProfissionaisPorUsuario(int usuarioId, bool? ativo)
        {
            try
            {
                return Ok(await _service.ListarExperienciaProfissionalPorUsuario(usuarioId, ativo));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{experienciaProfissionalId}")]
        public async Task<IActionResult> BuscarExperienciaProfissional(int experienciaProfissionalId)
        {
            try
            {
                return Ok(await _service.BuscarExperienciaProfissional(experienciaProfissionalId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarExperienciaProfissional([FromBody] AdicionarExperienciaProfissionalDto experienciaProfissionalDto)
        {
            try
            {
                await _service.CadastrarExperienciaProfissional(experienciaProfissionalDto);
                return StatusCode(201, "Experiência profissional cadastrada com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{experienciaProfissionalId}")]
        public async Task<IActionResult> AtualizarExperienciaProfissional(int experienciaProfissionalId, [FromBody] AtualizarExperienciaProfissionalDto experienciaProfissionalDto)
        {
            try
            {
                await _service.AtualizarExperienciaProfissional(experienciaProfissionalId, experienciaProfissionalDto);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoverExperienciaProfissional(int experienciaId)
        {
            try
            {
                await _service.RemoverExperienciaProfissional(experienciaId);
                return StatusCode(204, "Experiencia removida com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

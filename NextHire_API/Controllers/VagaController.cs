using Microsoft.AspNetCore.Mvc;
using NextHireAPI.Applications.Services;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHire_API.DTOs.VagaDTO;

namespace NextHireAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VagaController : ControllerBase
    {
        private readonly VagaService _service;

        public VagaController(VagaService service) => _service = service;

        [HttpGet("vagas")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                return Ok(await _service.Listar());
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("empresa/{empresaId}")]
        public async Task<IActionResult> ListarPorEmpresaId(int empresaId)
        {
            try
            {
                return Ok(await _service.ListarPorEmpresaId(empresaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("titulo")]
        public async Task<IActionResult> ListarPorTitulo(string titulo)
        {
            try
            {
                return Ok(await _service.ListarPorTitulo(titulo));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("status")]
        public async Task<IActionResult> ListarPorStatus(bool status)
        {
            try
            {
                return Ok(await _service.ListarPorStatus(status));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{vagaId}")]
        public async Task<IActionResult> BuscarPorId(int vagaId)
        {
            try
            {
                return Ok(await _service.BuscarPorId(vagaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarVagaDto vaga)
        {
            try
            {
                await _service.Adicionar(vaga);

                return StatusCode(201, "Vaga cadastrada com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{vagaId}")]
        public async Task<IActionResult> Atualizar(int vagaId, [FromBody] AtualizarVagaDto vaga)
        {
            try
            {
                await _service.Atualizar(vagaId, vaga);

                return StatusCode(204, "Vaga atualizada com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{vagaId}")]
        public async Task<IActionResult> Remover(int vagaId)
        {
            try
            {
                await _service.Remover(vagaId);

                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
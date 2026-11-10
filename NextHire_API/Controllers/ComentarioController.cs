using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.ComentarioDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly ComentarioService _service;

        public ComentarioController(ComentarioService service) => _service = service;

        [HttpGet("comentarios")]
        public async Task<IActionResult> ListarComentarios()
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

        [HttpGet("entrevista/{entrevistaId}")]
        public async Task<IActionResult> ListarPorEntrevistaId(int entrevistaId)
        {
            try
            {
                return Ok(await _service.ListarPorEntrevistaId(entrevistaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("gestor/{gestorId}")]
        public async Task<IActionResult> ListarPorGestorId(int gestorId)
        {
            try
            {
                return Ok(await _service.ListarPorGestorId(gestorId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{comentarioId}")]
        public async Task<IActionResult> BuscarPorId(int comentarioId)
        {
            try
            {
                return Ok(await _service.BuscarPorId(comentarioId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarComentario(
            [FromBody] AdicionarComentarioDto comentario)
        {
            try
            {
                await _service.Adicionar(comentario);

                return StatusCode(201, "Comentário cadastrado com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{comentarioId}")]
        public async Task<IActionResult> AtualizarComentario(
            int comentarioId,
            [FromBody] AtualizarComentarioDto comentarioAtualizado)
        {
            try
            {
                await _service.Atualizar(comentarioAtualizado, comentarioId);

                return StatusCode(204, "Comentário atualizado com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{comentarioId}")]
        public async Task<IActionResult> RemoverComentario(int comentarioId)
        {
            try
            {
                await _service.Remover(comentarioId);

                return StatusCode(204, "Comentário removido com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.AvaliacaoHabilidadeDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoHabilidadeController : ControllerBase
    {
        private readonly AvaliacaoHabilidadeService _service;

        public AvaliacaoHabilidadeController(AvaliacaoHabilidadeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                return Ok(await _service.Listar());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("candidato/{candidatoId:int}")]
        public async Task<IActionResult> BuscarPorCandidato(int candidatoId)
        {
            try
            {
                return Ok(await _service.BuscarPorCandidato(candidatoId));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{candidatoId:int}/{entrevistaId:int}/{habilidadeId:int}")]
        public async Task<IActionResult> BuscarPorChave(int candidatoId, int entrevistaId, int habilidadeId)
        {
            try
            {
                return Ok(await _service.BuscarPorChave(candidatoId, entrevistaId, habilidadeId));
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
        public async Task<IActionResult> Adicionar([FromBody] AdicionarAvaliacaoHabilidadeDto dto)
        {
            try
            {
                var avaliacao = await _service.Adicionar(dto);
                return CreatedAtAction(
                    nameof(BuscarPorChave),
                    new
                    {
                        candidatoId = avaliacao.CandidatoId,
                        entrevistaId = avaliacao.EntrevistaId,
                        habilidadeId = avaliacao.HabilidadeId
                    },
                    avaliacao);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{candidatoId:int}/{entrevistaId:int}/{habilidadeId:int}")]
        public async Task<IActionResult> Atualizar(
            int candidatoId,
            int entrevistaId,
            int habilidadeId,
            [FromBody] AtualizarAvaliacaoHabilidadeDto dto)
        {
            try
            {
                return Ok(await _service.Atualizar(candidatoId, entrevistaId, habilidadeId, dto));
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

        [HttpDelete("{candidatoId:int}/{entrevistaId:int}/{habilidadeId:int}")]
        public async Task<IActionResult> Deletar(int candidatoId, int entrevistaId, int habilidadeId)
        {
            try
            {
                await _service.Deletar(candidatoId, entrevistaId, habilidadeId);
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

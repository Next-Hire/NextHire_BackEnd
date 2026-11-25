using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.EntrevistaDTO;
using NextHire_API.DTOs.Setor_OcupacaoDTOs; 
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntrevistaController : ControllerBase
    {
        private readonly EntrevistaService _service;

        public EntrevistaController(EntrevistaService service)
        {
            _service = service;
        }

        // GET: api/Entrevista
        [HttpGet]
        public async Task<ActionResult<List<LerEntrevistaDTO>>> Listar()
        {
            try
            {
                List<LerEntrevistaDTO> entrevistas = await _service.Listar();
                return Ok(entrevistas);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // GET: api/Entrevista/status?status=true
        [HttpGet("status")]
        public async Task<ActionResult<List<LerEntrevistaDTO>>> ListarPorStatus(bool status)
        {
            try
            {
                List<LerEntrevistaDTO> entrevistas =
                    await _service.ListarPorStatus(status);
                return Ok(entrevistas);
            }
            catch (DomainException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        // GET: api/Entrevista/hoje
        [HttpGet("hoje")]
        public async Task<ActionResult<List<LerEntrevistaDTO>>> EntrevistasHoje()
        {
            try
            {
                List<LerEntrevistaDTO> entrevistas = await _service.EntrevistasHoje();
                return Ok(entrevistas);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // GET: api/Entrevista/candidato/5
        [HttpGet("candidato/{candidatoId:int}")]
        public async Task<ActionResult<List<LerEntrevistaDTO>>> BuscarPorCandidato(int candidatoId)
        {
            try
            {
                List<LerEntrevistaDTO> entrevistas = await _service.BuscarPorCandidato(candidatoId);
                return Ok(entrevistas);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // GET: api/Entrevista/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LerEntrevistaDTO>> BuscarPorId(int id)
        {
            try
            {
                LerEntrevistaDTO entrevista = await _service.BuscarPorId(id);
                return Ok(entrevista);
            }
            catch (DomainException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        // POST: api/Entrevista
        [HttpPost]
        public async Task<ActionResult<LerEntrevistaDTO>> AdicionarEntrevista([FromBody] CriarEntrevistaDTO dto)
        {
            try
            {
                LerEntrevistaDTO criada = await _service.AdicionarEntrevista(dto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = criada.entrevistaId }, criada);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // PATCH: api/Entrevista/5/candidato/10
        [HttpPatch("{entrevistaId:int}/candidato/{candidatoId:int}")]
        public async Task<ActionResult<LerEntrevistaDTO>> AtualizarCandidatoNaEntrevista(int entrevistaId, int candidatoId)
        {
            try
            {
                LerEntrevistaDTO atualizada = await _service.AtualizarCandidatoNaEntrevista(entrevistaId, candidatoId);
                return Ok(atualizada);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // PATCH: api/Entrevista/5/data
        [HttpPatch("{entrevistaId:int}/data")]
        public async Task<ActionResult<LerEntrevistaDTO>> AtualizarDataDaEntrevista(
            int entrevistaId,
            [FromBody] AtualizarDataEntrevistaDTO dto)
        {
            try
            {
                LerEntrevistaDTO atualizada = await _service.AtualizarDataDaEntrevista(
                    entrevistaId,
                    dto.dataInicio,
                    dto.dataEncerra);

                return Ok(atualizada);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // DELETE: api/Entrevista/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<LerEntrevistaDTO>> RemoverEntrevista(int id)
        {
            try
            {
                LerEntrevistaDTO removida = await _service.RemoverEntrevista(id);
                return Ok(removida);
            }
            catch (DomainException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
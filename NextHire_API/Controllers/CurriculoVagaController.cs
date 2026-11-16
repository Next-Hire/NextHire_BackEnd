using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.CandidatoVagaDTO;
using NextHire_API.DTOs.VagaDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CurriculoVagaController : ControllerBase
    {
        private readonly CurriculoVagaService _service;

        public CurriculoVagaController(CurriculoVagaService service) => _service = service;

        /// <summary>
        /// Lista os candidatos que se candidataram a uma vaga.
        /// </summary>
        [HttpGet("CandidatosPorVagaId/{vagaId}")]
        public async Task<ActionResult<List<ListarCurriculoVagaDto>>> ListarCurriculosPorVagaId(int vagaId)
        {
            try
            {
                return Ok(await _service.ListarCandidatosPorVagaId(vagaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Lista as vagas em que um candidato se candidatou.
        /// </summary>
        [HttpGet("VagasPorCandidatoId/{candidatoId}")]
        public async Task<ActionResult<List<ListarVagaDto>>> ListarVagasPorCandidatoId(int candidatoId)
        {
            try
            {
                return Ok(await _service.ListarVagaPorCandidatoId(candidatoId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Efetua a candidatura de um currículo a uma vaga.
        /// </summary>
        [HttpPost("EfetuarCandidatura/curriculo/{curriculoId}/vaga/{vagaId}")]
        public async Task<ActionResult> EfetuarCandidatura(int curriculoId, int vagaId)
        {
            try
            {
                await _service.EfetuarCandidatura(curriculoId, vagaId);
                return StatusCode(201, "Candidatura enviada.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cancela a candidatura de um currículo a uma vaga.
        /// </summary>
        [HttpDelete("CancelarCandidatura/curriculo/{curriculoId}/vaga/{vagaId}")]
        public async Task<ActionResult> CancelarCandidatura(int curriculoId, int vagaId)
        {
            try
            {
                await _service.CancelarCandidatura(curriculoId, vagaId);
                return Ok("Candidatura cancelada.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

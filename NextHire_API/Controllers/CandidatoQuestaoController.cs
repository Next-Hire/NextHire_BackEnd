
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.Domains;
using NextHire_API.DTOs.CandidatoQuestaoDTO;
using NextHire_API.Exceptions;
using NextHireAPI.Applications.Services;
using NextHireAPI.Domains;
using System.Collections.Concurrent;
using System.Net.Mail;


namespace NextHireAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CandidatoQuestaoController : ControllerBase
    {
        private readonly CandidatoQuestaoService _candQuestaoService;

        public CandidatoQuestaoController(CandidatoQuestaoService candQuestaoService)
        {
            _candQuestaoService = candQuestaoService;
        }

        // GET: api/CandidatoQuestao/existe?questaoId=1&candidatoId=10
        [HttpGet("existe")]
        public async Task<IActionResult> ExisteResposta([FromQuery] int questaoId, [FromQuery] int candidatoId)
        {
            var existe = await _candQuestaoService.ExisteRespostaCandidato(questaoId, candidatoId);
            return Ok(existe);
        }

        // GET: api/CandidatoQuestao/questao/1/candidato/10
        [HttpGet("questao/{questaoId}/candidato/{candidatoId}")]
        public async Task<ActionResult<LerCandidatoQuestaoDTO>> BuscarPorId(int questaoId, int candidatoId)
        {
            try
            {
                LerCandidatoQuestaoDTO? response = await _candQuestaoService.BuscarPorId(questaoId, candidatoId);
                return Ok(response);
            }
            catch (DomainException e)
            {
                return NotFound(e.Message);
            }

        }

        // GET: api/CandidatoQuestao/candidato/10
        [HttpGet("candidato/{candidatoId}")]
        public async Task<ActionResult<List<LerCandidatoQuestaoDTO>>> ListarPorCandidato(int candidatoId)
        {
            try
            {
                List<LerCandidatoQuestaoDTO> lista = await _candQuestaoService.ListarPorCandidato(candidatoId);
                return Ok(lista);
            }

            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }

        }

        // GET: api/CandidatoQuestao/questao/1
        [HttpGet("questao/{questaoId}")]
        public async Task<ActionResult<List<LerCandidatoQuestaoDTO>>> ListarPorQuestao(int questaoId)
        {

            try
            {
                List<LerCandidatoQuestaoDTO> lista = await _candQuestaoService.ListarPorQuestao(questaoId);
                return Ok(lista);
            }

            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }

        }

        // GET: api/CandidatoQuestao/candidato/10/vaga/5
        [HttpGet("candidato/{candidatoId}/vaga/{vagaId}")]
        public async Task<ActionResult<List<LerCandidatoQuestaoDTO>>> ListarCandidatoEVaga(int candidatoId, int vagaId)
        {
            try
            {
                List<LerCandidatoQuestaoDTO> lista = await _candQuestaoService.ListarCandidatoEVaga(candidatoId, vagaId);
                return Ok(lista);
            }

            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/CandidatoQuestao/em-branco/candidato/10/vaga/5
        [HttpGet("em-branco/candidato/{candidatoId}/vaga/{vagaId}")]
        public async Task<ActionResult<List<Questao?>>> ListarPorQuestoesEmBranco(int candidatoId, int vagaId)
        {
            try
            {
                var query = await _candQuestaoService.ListarPorQuestoesEmBranco(candidatoId, vagaId);
                return Ok(query);
            }

            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
 
        }

        // // GET: api/CandidatoQuestao/gabarito/candidato/10?vagaId=5 (vagaId opcional)
        // [HttpGet("gabarito/candidato/{candidatoId}")]
        // public async Task<IActionResult> ListarRespostasComGabarito(int candidatoId, [FromQuery] int? vagaId = null)
        // {
        //     var lista = await _candQuestaoService.ListarRespostasComGabarito(candidatoId, vagaId);
        //     return Ok(lista);
        // }

        // GET: api/CandidatoQuestao/certas/candidato/10
        [HttpGet("certas/candidato/{candidatoId}")]
        public async Task<ActionResult<List<LerCandidatoQuestaoGabaritoDTO>>> ListarRepostasCertas(int candidatoId)
        {
            try
            {
                List<LerCandidatoQuestaoGabaritoDTO> query = await _candQuestaoService.ListarRepostasCertas(candidatoId);
                return Ok(query);
            }

            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/CandidatoQuestao/acertos/candidato/10/vaga/5
        // [HttpGet("acertos/candidato/{candidatoId}/vaga/{vagaId}")]
        // public async Task<IActionResult> ListaDeAcertos(int candidatoId, int vagaId)
        // {
        //     var acertos = await _candQuestaoService.ListaDeAcertos(candidatoId, vagaId);
        //     return Ok(new
        //     {
        //         totalAcertos = acertos.Count,
        //         questoesAcertadas = acertos
        //     });
        // }

        // POST: api/CandidatoQuestao
        [HttpPost]
        public async Task<ActionResult<LerCandidatoQuestaoDTO>> Adicionar([FromBody] CriarCandidatoQuestaoDTO dto)
        {

            try
            {
                LerCandidatoQuestaoDTO criar = await _candQuestaoService.Adicionar(dto);
                return Created();
            }

            catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/CandidatoQuestao/candidato/10/questao/1
        [HttpDelete("candidato/{candidatoId}/questao/{questaoId}")]
        public async Task<ActionResult<LerCandidatoQuestaoDTO>> Excluir(int candidatoId, int questaoId)
        {
            try
            {
                LerCandidatoQuestaoDTO? query = await _candQuestaoService.Excluir(candidatoId, questaoId);
                return Ok(query);
            }

            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
 
        }
    }
}

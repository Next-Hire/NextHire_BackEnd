
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using NextHireAPI.Applications.Services;
using NextHire_API.Exceptions;
using System.Net.Mail;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.CandidatoQuestaoDTO;
using NextHireAPI.Domains;


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
        public async Task<IActionResult> BuscarPorId(int questaoId, int candidatoId)
        {
            try
            {
                var response = await _candQuestaoService.BuscarPorId(questaoId, candidatoId);
                return Ok(response);
            }
            catch (DomainException e)
            {
                return NotFound(e.Message);
            }

        }

        // GET: api/CandidatoQuestao/candidato/10
        [HttpGet("candidato/{candidatoId}")]
        public async Task<IActionResult> ListarPorCandidato(int candidatoId)
        {
            var lista = await _candQuestaoService.ListarPorCandidato(candidatoId);
            return Ok(lista);
        }

        // GET: api/CandidatoQuestao/questao/1
        [HttpGet("questao/{questaoId}")]
        public async Task<IActionResult> ListarPorQuestao(int questaoId)
        {
            var lista = await _candQuestaoService.ListarPorQuestao(questaoId);
            return Ok(lista);
        }

        // GET: api/CandidatoQuestao/candidato/10/vaga/5
        [HttpGet("candidato/{candidatoId}/vaga/{vagaId}")]
        public async Task<IActionResult> ListarCandidatoEVaga(int candidatoId, int vagaId)
        {
            var lista = await _candQuestaoService.ListarCandidatoEVaga(candidatoId, vagaId);
            return Ok(lista);
        }

        // GET: api/CandidatoQuestao/em-branco/candidato/10/vaga/5
        [HttpGet("em-branco/candidato/{candidatoId}/vaga/{vagaId}")]
        public async Task<IActionResult> ListarPorQuestoesEmBranco(int candidatoId, int vagaId)
        {
            var questoes = await _candQuestaoService.ListarPorQuestoesEmBranco(candidatoId, vagaId);
            return Ok(questoes);
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
        public async Task<IActionResult> ListarRepostasCertas(int candidatoId)
        {
            var lista = await _candQuestaoService.ListarRepostasCertas(candidatoId);
            return Ok(lista);
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
        public async Task<IActionResult> Adicionar([FromBody] CriarCandidatoQuestaoDTO dto)
        {
            try
            {
                var criada = await _candQuestaoService.Adicionar(dto);
                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { questaoId = criada.questaoId, candidatoId = criada.candidatoId },
                    criada
                );
            }
            catch (InvalidOperationException ex)
            {
                // Retorna 400 Bad Request se o candidato já tiver respondido à questão
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // DELETE: api/CandidatoQuestao/candidato/10/questao/1
        [HttpDelete("candidato/{candidatoId}/questao/{questaoId}")]
        public async Task<IActionResult> Excluir(int candidatoId, int questaoId)
        {
            var excluida = await _candQuestaoService.Excluir(candidatoId, questaoId);
            if (excluida == null)
                return NotFound(new { mensagem = "Resposta não encontrada para exclusão." });

            return Ok(excluida);
        }
    }
}

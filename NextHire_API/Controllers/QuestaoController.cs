using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.QuestaoDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QuestaoController : ControllerBase
    {
        private readonly QuestaoService _service;

        public QuestaoController(QuestaoService service) => _service = service;

        /// <summary>
        /// Lista todas as questões de uma vaga (com gabarito).
        /// </summary>
        [HttpGet("vaga/{vagaId:int}")]
        public async Task<ActionResult<List<ListarQuestaoDto>>> ListarPorVaga(int vagaId)
        {
            try
            {
                return Ok(await _service.ListarPorVaga(vagaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Busca uma questão pelo ID (com gabarito).
        /// </summary>
        [HttpGet("{questaoId:int}")]
        public async Task<ActionResult<ListarQuestaoDto>> BuscarPorId(int questaoId)
        {
            try
            {
                return Ok(await _service.BuscarPorId(questaoId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gera 10 questões para a vaga via Next Agent (LM Studio) e as cadastra.
        /// </summary>
        [HttpPost("vaga/{vagaId:int}/gerar")]
        public async Task<ActionResult<List<ListarQuestaoDto>>> GerarQuestoes(int vagaId)
        {
            try
            {
                var questoes = await _service.GerarQuestoesDaVaga(vagaId);
                return StatusCode(201, questoes);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gera 5 questões novas para a vaga via Next Agent, sem repetir as existentes.
        /// </summary>
        [HttpPost("vaga/{vagaId:int}/gerar-mais")]
        public async Task<ActionResult<List<ListarQuestaoDto>>> GerarMaisQuestoes(int vagaId)
        {
            try
            {
                var questoes = await _service.GerarMaisQuestoesDaVaga(vagaId);
                return StatusCode(201, questoes);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cadastra uma questão manualmente na vaga.
        /// </summary>
        [HttpPost("vaga/{vagaId:int}")]
        public async Task<ActionResult<ListarQuestaoDto>> Adicionar(int vagaId, [FromBody] AdicionarQuestaoDto questao)
        {
            try
            {
                var criada = await _service.Adicionar(vagaId, questao);
                return StatusCode(201, criada);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza texto, classificação, tipo e/ou alternativas de uma questão.
        /// </summary>
        [HttpPatch("{questaoId:int}")]
        public async Task<IActionResult> Atualizar(int questaoId, [FromBody] AtualizarQuestaoDto questao)
        {
            try
            {
                await _service.Atualizar(questaoId, questao);
                return StatusCode(204, "Questão atualizada com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove uma questão (bloqueado se já houver respostas de candidatos).
        /// </summary>
        [HttpDelete("{questaoId:int}")]
        public async Task<IActionResult> Remover(int questaoId)
        {
            try
            {
                await _service.Remover(questaoId);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Verifica as respostas do candidato comparando com o gabarito das questões da vaga.
        /// acertou: true/false para objetivas e null para dissertativas.
        /// </summary>
        [HttpGet("verificar/candidato/{candidatoId:int}/vaga/{vagaId:int}")]
        public async Task<ActionResult<List<VerificarQuestaoDto>>> VerificarRespostas(int candidatoId, int vagaId)
        {
            try
            {
                return Ok(await _service.VerificarRespostas(candidatoId, vagaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

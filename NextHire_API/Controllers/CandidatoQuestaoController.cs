
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
    /// <summary>Endpoints de gerenciamento das respostas dos candidatos às questões das vagas.</summary>
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
        /// <summary>Verifica se o candidato já respondeu uma determinada questão.</summary>
        /// <param name="questaoId">ID da questão a ser verificada.</param>
        /// <param name="candidatoId">ID do candidato a ser verificado.</param>
        /// <returns>true se o candidato já respondeu a questão; false caso contrário.</returns>
        [HttpGet("existe")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Verifica se o candidato já respondeu a questão", Description = "Retorna true caso já exista resposta do candidato informado para a questão informada, e false caso contrário.")]
        [SwaggerResponse(200, "Verificação realizada com sucesso.", typeof(bool))]
        public async Task<IActionResult> ExisteResposta([FromQuery] int questaoId, [FromQuery] int candidatoId)
        {
            var existe = await _candQuestaoService.ExisteRespostaCandidato(questaoId, candidatoId);
            return Ok(existe);
        }

        // GET: api/CandidatoQuestao/questao/1/candidato/10
        /// <summary>Busca a resposta de um candidato para uma questão específica.</summary>
        /// <param name="questaoId">ID da questão desejada.</param>
        /// <param name="candidatoId">ID do candidato cuja resposta será buscada.</param>
        /// <returns>Dados da resposta do candidato: questão, tipo, dados do candidato, resposta e alternativas (sem gabarito).</returns>
        [HttpGet("questao/{questaoId}/candidato/{candidatoId}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Busca a resposta de uma questão de um candidato", Description = "Retorna a resposta do candidato para a questão informada, com enunciado, tipo, dados do candidato e alternativas sem gabarito.")]
        [SwaggerResponse(200, "Resposta encontrada.", typeof(LerCandidatoQuestaoDTO))]
        [SwaggerResponse(404, "Resposta não encontrada para a questão e candidato informados.")]
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
        /// <summary>Lista todas as respostas registradas de um candidato.</summary>
        /// <param name="candidatoId">ID do candidato cujas respostas serão listadas.</param>
        /// <returns>Lista de respostas do candidato, com questões, tipos e alternativas (sem gabarito).</returns>
        [HttpGet("candidato/{candidatoId}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista todas as respostas de um candidato", Description = "Retorna todas as respostas registradas pelo candidato informado, com enunciado, tipo das questões e alternativas sem gabarito.")]
        [SwaggerResponse(200, "Respostas listadas com sucesso.", typeof(List<LerCandidatoQuestaoDTO>))]
        [SwaggerResponse(404, "Nenhuma resposta encontrada para o candidato informado.")]
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
        /// <summary>Lista todas as respostas registradas para uma questão específica.</summary>
        /// <param name="questaoId">ID da questão cujas respostas serão listadas.</param>
        /// <returns>Lista de respostas de todos os candidatos para a questão informada.</returns>
        [HttpGet("questao/{questaoId}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista todas as respostas de uma questão", Description = "Retorna as respostas de todos os candidatos para a questão informada, com dados dos candidatos e alternativas sem gabarito.")]
        [SwaggerResponse(200, "Respostas listadas com sucesso.", typeof(List<LerCandidatoQuestaoDTO>))]
        [SwaggerResponse(404, "Nenhuma resposta encontrada para a questão informada.")]
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
        /// <summary>Lista as respostas de um candidato filtradas por uma vaga específica.</summary>
        /// <param name="candidatoId">ID do candidato cujas respostas serão listadas.</param>
        /// <param name="vagaId">ID da vaga cujas questões serão filtradas.</param>
        /// <returns>Lista de respostas do candidato para as questões da vaga informada.</returns>
        [HttpGet("candidato/{candidatoId}/vaga/{vagaId}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista as respostas de um candidato por vaga", Description = "Retorna as respostas do candidato informado às questões vinculadas à vaga informada.")]
        [SwaggerResponse(200, "Respostas listadas com sucesso.", typeof(List<LerCandidatoQuestaoDTO>))]
        [SwaggerResponse(404, "Nenhuma resposta encontrada para o candidato e vaga informados.")]
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
        /// <summary>Lista as questões de uma vaga que o candidato ainda não respondeu.</summary>
        /// <param name="candidatoId">ID do candidato a ser verificado.</param>
        /// <param name="vagaId">ID da vaga cujas questões serão verificadas.</param>
        /// <returns>Lista de questões sem resposta do candidato na vaga informada.</returns>
        [HttpGet("em-branco/candidato/{candidatoId}/vaga/{vagaId}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista as questões em branco de um candidato por vaga", Description = "Retorna as questões da vaga informada que ainda não foram respondidas pelo candidato informado.")]
        [SwaggerResponse(200, "Questões listadas com sucesso.", typeof(List<Questao>))]
        [SwaggerResponse(404, "Nenhuma questão em branco encontrada para o candidato e vaga informados.")]
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
        /// <summary>Lista as respostas certas de um candidato, com gabarito e correção.</summary>
        /// <param name="candidatoId">ID do candidato cujas respostas certas serão listadas.</param>
        /// <returns>Lista de respostas corretas do candidato, com gabarito das alternativas e indicador de acerto.</returns>
        [HttpGet("certas/candidato/{candidatoId}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista as respostas certas de um candidato", Description = "Retorna as respostas corretas do candidato informado, incluindo o gabarito das alternativas e o indicador de acerto (nulo para questões dissertativas).")]
        [SwaggerResponse(200, "Respostas certas listadas com sucesso.", typeof(List<LerCandidatoQuestaoGabaritoDTO>))]
        [SwaggerResponse(404, "Nenhuma resposta certa encontrada para o candidato informado.")]
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
        /// <summary>Salva a resposta de um candidato a uma questão de vaga.</summary>
        /// <param name="dto">Dados da resposta: questão, candidato e payload da resposta (resposta simples ou lista de alternativas para múltipla escolha).</param>
        /// <returns>Status 201 indicando que a resposta foi registrada.</returns>
        [HttpPost]
        [Authorize(Roles = "Candidato,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Salva a resposta de um candidato a uma questão", Description = "Registra a resposta do candidato à questão informada. Aceita resposta simples (verdadeiro/falso ou texto) ou lista de alternativas para múltipla escolha. Requer perfil de Candidato, Analista de RH ou Administrador.")]
        [SwaggerResponse(201, "Resposta registrada com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos: questão, candidato ou payload da resposta incorretos.")]
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
        /// <summary>Remove a resposta de um candidato a uma questão específica.</summary>
        /// <param name="candidatoId">ID do candidato cuja resposta será removida.</param>
        /// <param name="questaoId">ID da questão cuja resposta será removida.</param>
        /// <returns>Dados da resposta removida.</returns>
        [HttpDelete("candidato/{candidatoId}/questao/{questaoId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Remove a resposta de um candidato a uma questão", Description = "Exclui a resposta do candidato informado à questão informada e retorna os dados da resposta removida. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Resposta removida com sucesso.", typeof(LerCandidatoQuestaoDTO))]
        [SwaggerResponse(404, "Resposta não encontrada para o candidato e questão informados.")]
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

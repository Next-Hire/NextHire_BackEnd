using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.CandidatoVagaDTO;
using NextHire_API.DTOs.VagaDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de candidaturas: vínculo entre currículos e vagas.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CurriculoVagaController : ControllerBase
    {
        private readonly CurriculoVagaService _service;

        public CurriculoVagaController(CurriculoVagaService service) => _service = service;

        /// <summary>Lista os candidatos que se candidataram a uma vaga.</summary>
        /// <param name="vagaId">ID da vaga cujos candidatos serão listados.</param>
        /// <returns>Lista de currículos (candidaturas) vinculados à vaga informada.</returns>
        [HttpGet("CandidatosPorVagaId/{vagaId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista os candidatos de uma vaga", Description = "Retorna os currículos inscritos na vaga informada. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Candidatos listados com sucesso.", typeof(List<ListarCurriculoVagaDto>))]
        [SwaggerResponse(404, "Vaga não encontrada ou nenhuma candidatura registrada.")]
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

        /// <summary>Lista as vagas em que um candidato se candidatou.</summary>
        /// <param name="candidatoId">ID do candidato cujas candidaturas serão listadas.</param>
        /// <returns>Lista de vagas em que o candidato informado se inscreveu.</returns>
        [HttpGet("VagasPorCandidatoId/{candidatoId}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista as vagas de um candidato", Description = "Retorna as vagas em que o candidato informado se candidatou. Requer perfil de Candidato, Gestor, Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Vagas listadas com sucesso.", typeof(List<ListarVagaDto>))]
        [SwaggerResponse(404, "Candidato não encontrado ou nenhuma candidatura registrada.")]
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

        /// <summary>Efetua a candidatura de um currículo a uma vaga.</summary>
        /// <param name="curriculoId">ID do currículo que será vinculado à vaga.</param>
        /// <param name="vagaId">ID da vaga alvo da candidatura.</param>
        /// <returns>Mensagem de confirmação do envio da candidatura.</returns>
        [HttpPost("EfetuarCandidatura/curriculo/{curriculoId}/vaga/{vagaId}")]
        [Authorize(Roles = "Candidato")]
        [SwaggerOperation(Summary = "Efetua uma candidatura", Description = "Registra a candidatura do currículo informado à vaga informada. Requer perfil de Candidato.")]
        [SwaggerResponse(201, "Candidatura enviada.")]
        [SwaggerResponse(400, "Falha ao efetuar a candidatura (dados inválidos ou candidatura já existente).")]
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

        /// <summary>Cancela a candidatura de um currículo a uma vaga.</summary>
        /// <param name="curriculoId">ID do currículo cuja candidatura será cancelada.</param>
        /// <param name="vagaId">ID da vaga cuja candidatura será cancelada.</param>
        /// <returns>Mensagem de confirmação do cancelamento.</returns>
        [HttpDelete("CancelarCandidatura/curriculo/{curriculoId}/vaga/{vagaId}")]
        [Authorize(Roles = "Candidato")]
        [SwaggerOperation(Summary = "Cancela uma candidatura", Description = "Remove a candidatura do currículo informado à vaga informada. Requer perfil de Candidato.")]
        [SwaggerResponse(200, "Candidatura cancelada.")]
        [SwaggerResponse(400, "Falha ao cancelar a candidatura (candidatura inexistente ou dados inválidos).")]
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.AvaliacaoAgenteDTO;
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
        private readonly AvaliacaoAgenteService _avaliacaoService;
        private readonly ILogger<CurriculoVagaController> _logger;

        public CurriculoVagaController(
            CurriculoVagaService service,
            AvaliacaoAgenteService avaliacaoService,
            ILogger<CurriculoVagaController> logger)
        {
            _service = service;
            _avaliacaoService = avaliacaoService;
            _logger = logger;
        }

        /// <summary>Lista os candidatos que se candidataram a uma vaga.</summary>
        /// <param name="vagaId">ID da vaga cujos candidatos serão listados.</param>
        /// <returns>Lista de currículos (candidaturas) vinculados à vaga informada.</returns>
        [HttpGet("Candidaturas/empresa/{empresaId}")]
        //[Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista os candidaturas em vagas de uma empresa", Description = "Retorna os currículos inscritos nas vagas da empresa informada. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Candidatos listados com sucesso.", typeof(List<ListarCurriculoVagaDto>))]
        [SwaggerResponse(404, "Vaga não encontrada ou nenhuma candidatura registrada.")]
        public async Task<ActionResult<List<ListarCurriculoVagaDto>>> ListarTodasAsCandidaturasPorEmpresaId(int empresaId)
        {
            try
            {
                return Ok(await _service.ListarTodasAsCandidaturasPorEmpresaId(empresaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

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

        /// <summary>Efetua a candidatura de um currículo a uma vaga e gera a avaliação do agente.</summary>
        /// <param name="curriculoId">ID do currículo que será vinculado à vaga.</param>
        /// <param name="vagaId">ID da vaga alvo da candidatura.</param>
        /// <returns>Mensagem de confirmação do envio da candidatura.</returns>
        [HttpPost("EfetuarCandidatura/curriculo/{curriculoId}/vaga/{vagaId}")]
        [Authorize(Roles = "Candidato")]
        [SwaggerOperation(Summary = "Efetua uma candidatura", Description = "Registra a candidatura do currículo informado à vaga informada e, em seguida, gera e persiste a avaliação do Next Agent (nota + comentários). A candidatura é mantida mesmo se a avaliação falhar. Requer perfil de Candidato.")]
        [SwaggerResponse(201, "Candidatura enviada.")]
        [SwaggerResponse(400, "Falha ao efetuar a candidatura (dados inválidos ou candidatura já existente).")]
        public async Task<ActionResult> EfetuarCandidatura(int curriculoId, int vagaId)
        {
            try
            {
                await _service.EfetuarCandidatura(curriculoId, vagaId);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }

            // A candidatura já está persistida: uma falha do agente (LM Studio fora,
            // timeout ou resposta inválida) não deve desfazer nem invalidar a candidatura.
            try
            {
                await _avaliacaoService.AvaliarCandidaturaPorCurriculo(curriculoId, vagaId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Falha ao gerar a avaliação automática da candidatura (currículo {CurriculoId}, vaga {VagaId}).",
                    curriculoId,
                    vagaId);
            }

            return StatusCode(201, "Candidatura enviada.");
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

        /// <summary>Gera (e persiste) a avaliação do agente para uma candidatura.</summary>
        /// <param name="vagaId">ID da vaga da candidatura.</param>
        /// <param name="candidatoId">ID do candidato que se candidatou à vaga.</param>
        /// <returns>Nota geral e comentários do agente sobre as respostas e o currículo.</returns>
        [HttpPost("AvaliarCandidatura/vaga/{vagaId}/candidato/{candidatoId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Avalia uma candidatura com o Next Agent", Description = "Envia a vaga, as respostas do candidato e o currículo ao agente (LM Studio), valida a resposta e persiste nota geral + comentários. Se já existir avaliação para o par candidato/vaga, ela é atualizada. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Avaliação gerada e persistida.", typeof(ListarAvaliacaoAgenteDto))]
        [SwaggerResponse(400, "Dados inválidos, candidatura/respostas inexistentes ou resposta inválida do agente.")]
        public async Task<ActionResult<ListarAvaliacaoAgenteDto>> AvaliarCandidatura(int vagaId, int candidatoId)
        {
            try
            {
                return Ok(await _avaliacaoService.AvaliarCandidatura(vagaId, candidatoId));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Busca a avaliação do agente para uma candidatura.</summary>
        /// <param name="vagaId">ID da vaga da candidatura.</param>
        /// <param name="candidatoId">ID do candidato avaliado.</param>
        /// <returns>Avaliação persistida (nota geral e comentários).</returns>
        [HttpGet("Avaliacao/vaga/{vagaId}/candidato/{candidatoId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Busca a avaliação de uma candidatura", Description = "Retorna a avaliação do agente já persistida para a candidatura informada. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Avaliação encontrada.", typeof(ListarAvaliacaoAgenteDto))]
        [SwaggerResponse(404, "Nenhuma avaliação encontrada para o candidato e vaga informados.")]
        public async Task<ActionResult<ListarAvaliacaoAgenteDto>> BuscarAvaliacao(int vagaId, int candidatoId)
        {
            try
            {
                return Ok(await _avaliacaoService.Buscar(vagaId, candidatoId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Lista as avaliações do agente de uma vaga.</summary>
        /// <param name="vagaId">ID da vaga cujas avaliações serão listadas.</param>
        /// <returns>Avaliações persistidas para a vaga, da maior para a menor nota.</returns>
        [HttpGet("AvaliacoesPorVagaId/{vagaId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista as avaliações de uma vaga", Description = "Retorna as avaliações do agente já persistidas para a vaga informada, ordenadas por nota. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Avaliações listadas com sucesso.", typeof(List<ListarAvaliacaoAgenteDto>))]
        [SwaggerResponse(404, "Nenhuma avaliação encontrada para a vaga informada.")]
        public async Task<ActionResult<List<ListarAvaliacaoAgenteDto>>> ListarAvaliacoesPorVagaId(int vagaId)
        {
            try
            {
                return Ok(await _avaliacaoService.ListarPorVaga(vagaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

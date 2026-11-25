using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.CandidatoProcessoSeletivoDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de gerenciamento do vínculo e avanço de candidatos nos processos seletivos.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CandidatoProcessoSeletivoController : ControllerBase
    {
        private readonly CandidatoProcessoSeletivoService _service;
        public CandidatoProcessoSeletivoController(CandidatoProcessoSeletivoService service) => _service = service;

        /// <summary>Lista todos os vínculos entre candidatos e processos seletivos cadastrados.</summary>
        /// <returns>Lista completa de candidatos vinculados a processos seletivos.</returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista todos os candidatos e processos relacionados", Description = "Retorna todos os processos seletivos e seus respectivos candidatos vinculados no sistema.")]
        [SwaggerResponse(200, "Vínculos listados com sucesso.", typeof(List<LerCandidatoProcessoDTO>))]
        [SwaggerResponse(404, "Nenhum vínculo entre candidato e processo seletivo encontrado.")]
        public async Task<ActionResult<List<LerCandidatoProcessoDTO>>> Listar()
        {
            try
            {
                List<LerCandidatoProcessoDTO> lista = await _service.Listar();
                if (lista == null || lista.Count == 0)
                    return NotFound(lista);
                
                return Ok(lista);
            }
            catch (DomainException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        /// <summary>Lista todos os candidatos aprovados (com status Avança) em um processo seletivo.</summary>
        /// <param name="processoId">ID do processo seletivo desejado.</param>
        /// <returns>Lista de candidatos aprovados no processo seletivo informado.</returns>
        [HttpGet("aprovados/{processoId:int}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista os candidatos aprovados de um processo seletivo", Description = "Retorna todos os candidatos com status 'Avança' vinculados ao ID do processo seletivo informado.")]
        [SwaggerResponse(200, "Candidatos aprovados listados com sucesso.", typeof(List<LerCandidatoProcessoDTO>))]
        [SwaggerResponse(400, "ID inválido ou processo seletivo não encontrado.")]
        public async Task<ActionResult<List<LerCandidatoProcessoDTO>>> ListarAprovados(int processoId)
        {
            try
            {
                List<LerCandidatoProcessoDTO> aprovados = await _service.ListarAprovados(processoId);
                return Ok(aprovados);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>Busca um vínculo específico entre candidato e processo seletivo pelos seus IDs.</summary>
        /// <param name="candidatoId">ID do candidato desejado.</param>
        /// <param name="processoSeletivoId">ID do processo seletivo desejado.</param>
        /// <returns>Dados do vínculo entre o candidato e o processo seletivo informados.</returns>
        [HttpGet("candidato/{candidatoId:int}/processo/{processoSeletivoId:int}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca um vínculo pelo ID do candidato e do processo seletivo", Description = "Retorna os dados da relação correspondente à chave composta formada pelo ID do candidato e pelo ID do processo seletivo.")]
        [SwaggerResponse(200, "Vínculo encontrado com sucesso.", typeof(LerCandidatoProcessoDTO))]
        [SwaggerResponse(400, "IDs inválidos ou vínculo não encontrado.")]
        public async Task<ActionResult<LerCandidatoProcessoDTO>> BuscarPorId(int candidatoId, int processoSeletivoId)
        {
            try
            {
                LerCandidatoProcessoDTO relacao = await _service.BuscarPorId(candidatoId, processoSeletivoId);
                return Ok(relacao);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>Busca o vínculo correspondente ao ID de um processo seletivo.</summary>
        /// <param name="processoSeletivoId">ID do processo seletivo desejado.</param>
        /// <returns>Dados do vínculo encontrado para o processo seletivo informado.</returns>
        [HttpGet("processo/{processoSeletivoId:int}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca vínculo pelo ID do processo seletivo", Description = "Retorna os dados da relação vinculada ao ID do processo seletivo informado.")]
        [SwaggerResponse(200, "Vínculo encontrado com sucesso.", typeof(LerCandidatoProcessoDTO))]
        [SwaggerResponse(400, "ID inválido ou nenhum candidato vinculado ao processo seletivo.")]
        public async Task<ActionResult<LerCandidatoProcessoDTO>> BuscarPorProcessoSeletivoId(int processoSeletivoId)
        {
            try
            {
                LerCandidatoProcessoDTO relacao = await _service.BuscarPorProcessoSeletivoId(processoSeletivoId);
                return Ok(relacao);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>Busca o vínculo correspondente ao ID de um candidato.</summary>
        /// <param name="candidatoId">ID do candidato desejado.</param>
        /// <returns>Dados do vínculo encontrado para o candidato informado.</returns>
        [HttpGet("candidato/{candidatoId:int}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca vínculo pelo ID do candidato", Description = "Retorna os dados da relação vinculada ao ID do candidato informado.")]
        [SwaggerResponse(200, "Vínculo encontrado com sucesso.", typeof(LerCandidatoProcessoDTO))]
        [SwaggerResponse(400, "ID inválido ou nenhum processo seletivo vinculado ao candidato.")]
        public async Task<ActionResult<LerCandidatoProcessoDTO>> BuscarPorCandidatoId(int candidatoId)
        {
            try
            {
                LerCandidatoProcessoDTO relacao = await _service.BuscarPorCandidatoId(candidatoId);
                return Ok(relacao);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
        
        /// <summary>Busca todos os vínculos correspondentes ao ID de um processo seletivo.</summary>
        /// <param name="processoSeletivoId">ID do processo seletivo desejado.</param>
        /// <returns>Lista de vínculos encontrados para o processo seletivo informado.</returns>
        [HttpGet("processo/candidatos/{processoSeletivoId:int}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista vínculos pelo ID do processo seletivo", Description = "Retorna todos os candidatos vinculados ao ID do processo seletivo informado.")]
        [SwaggerResponse(200, "Vínculos encontrados com sucesso.", typeof(List<LerCandidatoProcessoDTO>))]
        [SwaggerResponse(400, "ID inválido ou nenhum candidato vinculado ao processo seletivo.")]
        public async Task<ActionResult<List<LerCandidatoProcessoDTO>>> BuscarCandidatosPorProcessoSeletivo(int processoSeletivoId)
        {
            try
            {
                List<LerCandidatoProcessoDTO> relacoes = await _service.BuscarCandidatosPorProcessoSeletivoId(processoSeletivoId);
                return Ok(relacoes);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>Vincula um candidato a um processo seletivo.</summary>
        /// <param name="dto">Dados do vínculo entre candidato e processo seletivo a ser criado.</param>
        /// <returns>Dados do vínculo criado.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrador,AnalistaRh,Gestor")]
        [SwaggerOperation(Summary = "Vincula um candidato a um processo seletivo", Description = "Cria a relação entre candidato e processo seletivo com o status inicial informado e retorna os dados criados. Requer perfil de Administrador, Analista de RH ou Gestor.")]
        [SwaggerResponse(201, "Candidato vinculado ao processo seletivo com sucesso.", typeof(LerCandidatoProcessoDTO))]
        [SwaggerResponse(400, "Dados inválidos, processo encerrado, vínculo duplicado ou erro ao cadastrar.")]
        public async Task<ActionResult<LerCandidatoProcessoDTO>> AdicionarCandidatoProcesso(
            [FromBody] CriarCandidatoProcessoDTO dto)
        {
            try
            {
                LerCandidatoProcessoDTO criado = await _service.AdicionarCandidatoProcesso(dto);
                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { candidatoId = criado.candidatoId, processoSeletivoId = criado.processoSeletivoId },
                    criado);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
        
        /// <summary>Vincula um ou mais candidatos a um processo seletivo em uma única requisição.</summary>
        /// <param name="dto">ID do processo seletivo, lista de IDs dos candidatos e status inicial.</param>
        /// <returns>Lista dos vínculos criados.</returns>
        [HttpPost("candidatos")]
        [Authorize(Roles = "Adm,AnalistaRh,Gestor")]
        [SwaggerOperation(Summary = "Vincula vários candidatos a um processo seletivo", Description = "Cria em lote a relação entre múltiplos candidatos e um processo seletivo com o status inicial informado. Requer perfil de Administrador, Analista de RH ou Gestor.")]
        [SwaggerResponse(200, "Candidatos vinculados ao processo seletivo com sucesso.", typeof(List<LerCandidatoProcessoDTO>))]
        [SwaggerResponse(400, "Dados inválidos, candidatos já vinculados ou erro ao cadastrar.")]
        public async Task<ActionResult<List<LerCandidatoProcessoDTO>>> AdicionarCandidatosProcessoEmLote(
            [FromBody] CriarCandidatosNoProcessoSeletivoDTO dto)
        {
            try
            {
                List<LerCandidatoProcessoDTO> criados = await _service.AdicionarCandidatosNoProcessoSeletivo(dto);
                return Created();
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>Atualiza o status da relação de um candidato em um processo seletivo.</summary>
        /// <param name="candidatoId">ID do candidato.</param>
        /// <param name="processoSeletivoId">ID do processo seletivo.</param>
        /// <param name="dto">Novo status da relação do candidato no processo seletivo.</param>
        /// <returns>Dados do vínculo atualizado.</returns>
        [HttpPut("candidato/{candidatoId}/processo/{processoSeletivoId}")]
        [Authorize(Roles = "Administrador,AnalistaRh,Gestor")]
        [SwaggerOperation(Summary = "Atualiza o status de um candidato no processo seletivo", Description = "Altera o status da relação (Considera, Avança ou Não Avança) entre o candidato e o processo seletivo informados. Requer perfil de Administrador, Analista de RH ou Gestor.")]
        [SwaggerResponse(200, "Status atualizado com sucesso.", typeof(LerCandidatoProcessoDTO))]
        [SwaggerResponse(400, "Dados inválidos, vínculo inexistente ou erro ao atualizar.")]
        public async Task<ActionResult<LerCandidatoProcessoDTO>> AtualizarCandidatoProcesso(
            int candidatoId,
            int processoSeletivoId,
            [FromBody] AtualizarCandidatoProcessoDTO dto)
        {
            try
            {
                LerCandidatoProcessoDTO atualizado = await _service.AtualizarCandidatoProcesso(
                    candidatoId,
                    processoSeletivoId,
                    dto);
                return Ok(atualizado);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>Remove o vínculo de um candidato com um processo seletivo.</summary>
        /// <param name="candidatoId">ID do candidato.</param>
        /// <param name="processoSeletivoId">ID do processo seletivo.</param>
        /// <returns>Dados do vínculo removido.</returns>
        [HttpDelete("candidato/{candidatoId:int}/processo/{processoSeletivoId:int}")]
        // [Authorize(Roles = "Administrador,AnalistaRh,Gestor")]
        [SwaggerOperation(Summary = "Remove um candidato de um processo seletivo", Description = "Exclui a relação entre o candidato e o processo seletivo informados e retorna os dados do vínculo removido. Requer perfil de Administrador, Analista de RH ou Gestor.")]
        [SwaggerResponse(200, "Vínculo removido com sucesso.", typeof(LerCandidatoProcessoDTO))]
        [SwaggerResponse(400, "Vínculo não encontrado ou erro ao remover.")]
        public async Task<ActionResult<LerCandidatoProcessoDTO>> RemoverCandidatoProcesso(
            int candidatoId,
            int processoSeletivoId)
        {
            try
            {
                LerCandidatoProcessoDTO removido = await _service.RemoverCandidatoProcesso(candidatoId, processoSeletivoId);
                return Ok(removido);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
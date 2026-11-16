using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.AvaliacaoHabilidadeDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de gerenciamento das avaliações de habilidades dos candidatos nas entrevistas.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AvaliacaoHabilidadeController : ControllerBase
    {
        private readonly AvaliacaoHabilidadeService _service;

        public AvaliacaoHabilidadeController(AvaliacaoHabilidadeService service)
        {
            _service = service;
        }

        /// <summary>Lista todas as avaliações de habilidades registradas no sistema.</summary>
        /// <returns>Lista completa de avaliações com dados do candidato, da entrevista e da habilidade avaliada.</returns>
        [HttpGet]
        [Authorize(Roles = "Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista todas as avaliações de habilidades", Description = "Retorna todas as avaliações de habilidades registradas, com dados do candidato, da entrevista e da habilidade avaliada.")]
        [SwaggerResponse(200, "Avaliações listadas com sucesso.", typeof(List<ListarAvaliacaoHabilidadeDto>))]
        [SwaggerResponse(500, "Erro interno ao listar as avaliações.")]
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

        /// <summary>Lista todas as avaliações de habilidades registradas para um candidato específico.</summary>
        /// <param name="candidatoId">ID do candidato cujas avaliações serão listadas.</param>
        /// <returns>Lista de avaliações do candidato informado, com dados da entrevista e da habilidade avaliada.</returns>
        [HttpGet("candidato/{candidatoId:int}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista as avaliações de habilidades de um candidato", Description = "Retorna todas as avaliações de habilidades registradas para o candidato informado, com dados da entrevista e da habilidade avaliada.")]
        [SwaggerResponse(200, "Avaliações listadas com sucesso.", typeof(List<ListarAvaliacaoHabilidadeDto>))]
        [SwaggerResponse(400, "ID de candidato inválido ou nenhuma avaliação encontrada.")]
        [SwaggerResponse(500, "Erro interno ao listar as avaliações.")]
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

        /// <summary>Busca uma avaliação de habilidade específica pela chave composta (candidato, entrevista e habilidade).</summary>
        /// <param name="candidatoId">ID do candidato avaliado.</param>
        /// <param name="entrevistaId">ID da entrevista em que a avaliação foi feita.</param>
        /// <param name="habilidadeId">ID da habilidade avaliada.</param>
        /// <returns>Dados da avaliação, incluindo o nível avaliado da habilidade.</returns>
        [HttpGet("{candidatoId:int}/{entrevistaId:int}/{habilidadeId:int}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Busca uma avaliação pela chave composta", Description = "Retorna a avaliação de habilidade identificada pelo conjunto candidato + entrevista + habilidade, incluindo o nível avaliado.")]
        [SwaggerResponse(200, "Avaliação encontrada.", typeof(ListarAvaliacaoHabilidadeDto))]
        [SwaggerResponse(404, "Avaliação não encontrada para a chave composta informada.")]
        [SwaggerResponse(400, "Dados inválidos na chave composta informada.")]
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

        /// <summary>Registra uma nova avaliação de habilidade para um candidato em uma entrevista.</summary>
        /// <param name="dto">Dados da avaliação: candidato, entrevista, habilidade e nível avaliado.</param>
        /// <returns>Dados da avaliação criada, com a chave composta no header Location.</returns>
        [HttpPost]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Registra uma nova avaliação de habilidade", Description = "Cria uma avaliação de habilidade vinculada a um candidato, uma entrevista e uma habilidade, registrando o nível avaliado. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(201, "Avaliação registrada com sucesso.", typeof(ListarAvaliacaoHabilidadeDto))]
        [SwaggerResponse(400, "Dados inválidos: candidato, entrevista, habilidade ou nível fora dos valores aceitos.")]
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

        /// <summary>Atualiza uma avaliação de habilidade existente (normalmente a nota/nível avaliado).</summary>
        /// <param name="candidatoId">ID do candidato avaliado.</param>
        /// <param name="entrevistaId">ID da entrevista em que a avaliação foi feita.</param>
        /// <param name="habilidadeId">ID da habilidade avaliada.</param>
        /// <param name="dto">Novos dados da avaliação.</param>
        /// <returns>Dados da avaliação atualizada.</returns>
        [HttpPut("{candidatoId:int}/{entrevistaId:int}/{habilidadeId:int}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Atualiza uma avaliação de habilidade", Description = "Atualiza a avaliação identificada pela chave composta candidato + entrevista + habilidade, normalmente alterando o nível (nota) avaliado. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Avaliação atualizada com sucesso.", typeof(ListarAvaliacaoHabilidadeDto))]
        [SwaggerResponse(404, "Avaliação não encontrada para a chave composta informada.")]
        [SwaggerResponse(400, "Dados inválidos na atualização da avaliação.")]
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

        /// <summary>Remove uma avaliação de habilidade do sistema.</summary>
        /// <param name="candidatoId">ID do candidato avaliado.</param>
        /// <param name="entrevistaId">ID da entrevista em que a avaliação foi feita.</param>
        /// <param name="habilidadeId">ID da habilidade avaliada.</param>
        /// <returns>Resposta vazia com status 204.</returns>
        [HttpDelete("{candidatoId:int}/{entrevistaId:int}/{habilidadeId:int}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Remove uma avaliação de habilidade", Description = "Exclui definitivamente a avaliação identificada pela chave composta candidato + entrevista + habilidade. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(204, "Avaliação removida com sucesso.")]
        [SwaggerResponse(404, "Avaliação não encontrada para a chave composta informada.")]
        [SwaggerResponse(400, "Erro ao remover a avaliação.")]
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

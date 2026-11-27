using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.ComentarioDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de gerenciamento dos comentários de gestores sobre entrevistas.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly ComentarioService _service;

        public ComentarioController(ComentarioService service) => _service = service;

        /// <summary>Lista todos os comentários cadastrados.</summary>
        /// <returns>Lista completa de comentários.</returns>
        [HttpGet("comentarios")]
        [Authorize(Roles = "Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista todos os comentários", Description = "Retorna todos os comentários cadastrados no sistema. Requer perfil de Gestor, Analista de RH ou Adm.")]
        [SwaggerResponse(200, "Comentários listados com sucesso.", typeof(List<ListarComentarioDto>))]
        [SwaggerResponse(404, "Nenhum comentário cadastrado.")]
        public async Task<IActionResult> ListarComentarios()
        {
            try
            {
                return Ok(await _service.Listar());
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Lista os comentários de uma entrevista específica.</summary>
        /// <param name="entrevistaId">ID da entrevista cujos comentários serão listados.</param>
        /// <returns>Comentários da entrevista informada.</returns>
        [HttpGet("entrevista/{entrevistaId}")]
        [Authorize(Roles = "Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista os comentários de uma entrevista", Description = "Retorna os comentários vinculados à entrevista informada. Requer perfil de Gestor, Analista de RH ou Adm.")]
        [SwaggerResponse(200, "Comentários listados com sucesso.", typeof(List<ListarComentarioDto>))]
        [SwaggerResponse(404, "Nenhum comentário encontrado para a entrevista informada.")]
        public async Task<IActionResult> ListarPorEntrevistaId(int entrevistaId)
        {
            try
            {
                return Ok(await _service.ListarPorEntrevistaId(entrevistaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Lista os comentários feitos por um gestor específico.</summary>
        /// <param name="gestorId">ID do gestor cujos comentários serão listados.</param>
        /// <returns>Comentários feitos pelo gestor informado.</returns>
        [HttpGet("gestor/{gestorId}")]
        [Authorize(Roles = "Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista os comentários de um gestor", Description = "Retorna os comentários feitos pelo gestor informado. Requer perfil de Gestor, Analista de RH ou Adm.")]
        [SwaggerResponse(200, "Comentários listados com sucesso.", typeof(List<ListarComentarioDto>))]
        [SwaggerResponse(404, "Nenhum comentário encontrado para o gestor informado.")]
        public async Task<IActionResult> ListarPorGestorId(int gestorId)
        {
            try
            {
                return Ok(await _service.ListarPorGestorId(gestorId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca um comentário específico pelo ID.</summary>
        /// <param name="comentarioId">ID do comentário desejado.</param>
        /// <returns>Dados do comentário encontrado.</returns>
        [HttpGet("{comentarioId}")]
        [Authorize(Roles = "Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Busca um comentário pelo ID", Description = "Retorna os dados do comentário correspondente ao ID informado. Requer perfil de Gestor, Analista de RH ou Adm.")]
        [SwaggerResponse(200, "Comentário encontrado.", typeof(ListarComentarioDto))]
        [SwaggerResponse(404, "Comentário não encontrado.")]
        public async Task<IActionResult> BuscarPorId(int comentarioId)
        {
            try
            {
                return Ok(await _service.BuscarPorId(comentarioId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Cadastra um novo comentário sobre uma entrevista.</summary>
        /// <param name="comentario">Dados do comentário: entrevista, gestor autor e conteúdo.</param>
        /// <returns>Mensagem de confirmação do cadastro.</returns>
        [HttpPost]
        [Authorize(Roles = "Gestor")]
        [SwaggerOperation(Summary = "Cadastra um novo comentário", Description = "Cria um comentário de gestor sobre uma entrevista. Requer perfil de Gestor.")]
        [SwaggerResponse(201, "Comentário cadastrado com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos ou erro ao cadastrar o comentário.")]
        public async Task<IActionResult> AdicionarComentario(
            [FromBody] AdicionarComentarioDto comentario)
        {
            try
            {
                await _service.Adicionar(comentario);

                return StatusCode(201, "Comentário cadastrado com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza o conteúdo de um comentário existente.</summary>
        /// <param name="comentarioId">ID do comentário a ser atualizado.</param>
        /// <param name="comentarioAtualizado">Novo conteúdo do comentário.</param>
        /// <returns>Mensagem de confirmação da atualização.</returns>
        [HttpPatch("{comentarioId}")]
        [Authorize(Roles = "Gestor,Adm")]
        [SwaggerOperation(Summary = "Atualiza um comentário", Description = "Substitui o conteúdo do comentário informado. Requer perfil de Gestor ou Adm.")]
        [SwaggerResponse(204, "Comentário atualizado com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos, comentário inexistente ou erro ao atualizar.")]
        public async Task<IActionResult> AtualizarComentario(
            int comentarioId,
            [FromBody] AtualizarComentarioDto comentarioAtualizado)
        {
            try
            {
                await _service.Atualizar(comentarioAtualizado, comentarioId);

                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove um comentário do sistema.</summary>
        /// <param name="comentarioId">ID do comentário a ser removido.</param>
        /// <returns>Mensagem de confirmação da remoção.</returns>
        [HttpDelete("{comentarioId}")]
        [Authorize(Roles = "Gestor,Adm")]
        [SwaggerOperation(Summary = "Remove um comentário", Description = "Exclui definitivamente o comentário informado. Requer perfil de Gestor ou Adm.")]
        [SwaggerResponse(204, "Comentário removido com sucesso.")]
        [SwaggerResponse(400, "Comentário não encontrado ou erro ao remover.")]
        public async Task<IActionResult> RemoverComentario(int comentarioId)
        {
            try
            {
                await _service.Remover(comentarioId);

                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
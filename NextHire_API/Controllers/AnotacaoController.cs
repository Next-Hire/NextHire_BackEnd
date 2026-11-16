using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.AnotacaoDto;
using Microsoft.AspNetCore.Mvc;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de gerenciamento das anotações privadas dos candidatos.</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnotacaoController : ControllerBase
    {
        private readonly AnotacaoService _service;

        public AnotacaoController(AnotacaoService anotacaoService)
        {
            _service = anotacaoService;
        }

        /// <summary>Lista todas as anotações cadastradas.</summary>
        /// <returns>Lista completa de anotações.</returns>
        [HttpGet]
        [Authorize(Roles = "Adm")]
        [SwaggerOperation(Summary = "Lista todas as anotações", Description = "Retorna todas as anotações cadastradas no sistema. Requer perfil de Administrador.")]
        [SwaggerResponse(200, "Anotações listadas com sucesso.", typeof(List<ListarAnotacaoDto>))]
        [SwaggerResponse(400, "Erro ao listar as anotações.")]
        public ActionResult Listar()
        {
            try
            {
                var anotacoes = _service.Listar();
                return Ok(anotacoes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Busca uma anotação específica pelo ID.</summary>
        /// <param name="id">ID da anotação desejada.</param>
        /// <returns>Dados da anotação encontrada.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Busca uma anotação pelo ID", Description = "Retorna os dados da anotação correspondente ao ID informado. Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(200, "Anotação encontrada.", typeof(ListarAnotacaoDto))]
        [SwaggerResponse(400, "Anotação não encontrada ou erro na busca.")]
        public ActionResult BuscarPorId(int id)
        {
            try
            {
                var anotacao = _service.BuscarPorId(id);
                return Ok(anotacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Cadastra uma nova anotação do candidato.</summary>
        /// <param name="anotacaoDto">Dados da anotação a ser criada.</param>
        /// <returns>Dados da anotação criada.</returns>
        [HttpPost]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Cadastra uma nova anotação", Description = "Cria uma anotação privada do candidato e retorna os dados da anotação criada. Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(200, "Anotação cadastrada com sucesso.", typeof(ListarAnotacaoDto))]
        [SwaggerResponse(400, "Dados inválidos ou erro ao cadastrar a anotação.")]
        public ActionResult Adicionar(AdicionarAnotacaoDto anotacaoDto)
        {
            try
            {
                var anotacao = _service.Adicionar(anotacaoDto);
                return Ok(anotacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza os dados de uma anotação existente.</summary>
        /// <param name="anotacaoDto">Novos dados da anotação.</param>
        /// <param name="id">ID da anotação a ser atualizada.</param>
        /// <returns>Dados da anotação atualizada.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Atualiza uma anotação", Description = "Substitui os dados da anotação informada e retorna os dados atualizados. Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(200, "Anotação atualizada com sucesso.", typeof(ListarAnotacaoDto))]
        [SwaggerResponse(400, "Dados inválidos, anotação inexistente ou erro ao atualizar.")]
        public ActionResult Atualizar(AdicionarAnotacaoDto anotacaoDto, int id)
        {
            try
            {
                var anotacao = _service.Atualizar(anotacaoDto, id);
                return Ok(anotacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove uma anotação do sistema.</summary>
        /// <param name="id">ID da anotação a ser removida.</param>
        /// <returns>Resposta vazia com status 200.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Remove uma anotação", Description = "Exclui definitivamente a anotação informada. Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(200, "Anotação removida com sucesso.")]
        [SwaggerResponse(400, "Anotação não encontrada ou erro ao remover.")]
        public ActionResult Deletar(int id)
        {
            try
            {
                _service.Deletar(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

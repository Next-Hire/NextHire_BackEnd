using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.AreaOcupacaoDto;
using System.Runtime.ConstrainedExecution;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de gerenciamento de áreas de ocupação, incluindo listagem, cadastro, atualização e remoção.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AreaOcupacaoController : ControllerBase
    {
        private readonly AreaOcupacaoService _service;

        public AreaOcupacaoController(AreaOcupacaoService areaOcupacaoService)
        {
            _service = areaOcupacaoService;
        }

        /// <summary>Lista todas as áreas de ocupação cadastradas.</summary>
        /// <returns>Lista completa de áreas de ocupação.</returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista todas as áreas de ocupação", Description = "Retorna o catálogo completo de áreas de ocupação. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(200, "Áreas de ocupação listadas com sucesso.")]
        [SwaggerResponse(400, "Erro ao listar as áreas de ocupação.")]
        public ActionResult Listar()
        {
            try
            {
                var areas = _service.Listar();
                return Ok(areas);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Busca uma área de ocupação pelo ID.</summary>
        /// <param name="id">ID da área de ocupação desejada.</param>
        /// <returns>Dados da área de ocupação encontrada.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca uma área de ocupação pelo ID", Description = "Retorna os dados da área de ocupação com o ID informado. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(200, "Área de ocupação encontrada.")]
        [SwaggerResponse(404, "Área de ocupação não encontrada.")]
        public ActionResult ListarPorId(int id)
        {
            try
            {
                var areaOcupacao = _service.ListarPorID(id);
                if (areaOcupacao == null)
                {
                    return NotFound();
                }
                return Ok(areaOcupacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Busca uma área de ocupação pelo nome.</summary>
        /// <param name="nome">Nome da área de ocupação desejada.</param>
        /// <returns>Dados da área de ocupação encontrada.</returns>
        [HttpGet("nome/{nome}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca uma área de ocupação pelo nome", Description = "Retorna os dados da área de ocupação com o nome informado. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(200, "Área de ocupação encontrada.")]
        [SwaggerResponse(404, "Área de ocupação não encontrada.")]
        public ActionResult ListarPorNome(string nome)
        {
            try
            {
                var areaOcupacao = _service.ListarPorNome(nome);
                if (areaOcupacao == null)
                {
                    return NotFound();
                }
                return Ok(areaOcupacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Cadastra uma nova área de ocupação.</summary>
        /// <param name="areaOcupacao">Dados da área de ocupação a ser cadastrada.</param>
        /// <returns>Dados da área de ocupação cadastrada.</returns>
        [HttpPost]
        [Authorize(Roles = "Adm")]
        [SwaggerOperation(Summary = "Cadastra uma nova área de ocupação", Description = "Cria uma nova área de ocupação no catálogo e retorna os dados dela. Requer perfil de Adm.")]
        [SwaggerResponse(201, "Área de ocupação cadastrada com sucesso.", typeof(ListarAreaOcupacaoDto))]
        [SwaggerResponse(400, "Dados inválidos ou área já cadastrada.")]
        public ActionResult Adicionar(AdicionarAreaOcupacao areaOcupacao)
        {
            try
            {
                ListarAreaOcupacaoDto area = _service.Adicionar(areaOcupacao);  
                return StatusCode(201, area);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza os dados de uma área de ocupação existente.</summary>
        /// <param name="id">ID da área de ocupação a ser atualizada.</param>
        /// <param name="areaOcupacao">Novos dados da área de ocupação.</param>
        /// <returns>Dados da área de ocupação atualizada.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Adm")]
        [SwaggerOperation(Summary = "Atualiza uma área de ocupação", Description = "Substitui os dados da área de ocupação informada. Requer perfil de Adm.")]
        [SwaggerResponse(204, "Área de ocupação atualizada com sucesso.")]
        [SwaggerResponse(400, "Área de ocupação não encontrada ou dados inválidos.")]
        public ActionResult Atualizar(int id, AdicionarAreaOcupacao areaOcupacao)
        {
            try
            {
                var area = _service.Atualizar(id, areaOcupacao);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove uma área de ocupação do catálogo.</summary>
        /// <param name="id">ID da área de ocupação a ser removida.</param>
        /// <returns>Resposta vazia com status 204.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Adm")]
        [SwaggerOperation(Summary = "Remove uma área de ocupação", Description = "Exclui definitivamente a área de ocupação informada do catálogo. Requer perfil de Adm.")]
        [SwaggerResponse(204, "Área de ocupação removida com sucesso.")]
        [SwaggerResponse(400, "Área de ocupação não encontrada ou erro ao remover.")]
        public ActionResult Excluir(int id)
        {
            try
            {
                _service.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
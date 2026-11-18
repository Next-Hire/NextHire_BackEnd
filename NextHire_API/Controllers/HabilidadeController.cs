using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.HabilidadeDto;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de gerenciamento do catálogo de habilidades.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HabilidadeController : ControllerBase
    {
        private readonly HabilidadesService _service;
        public HabilidadeController(HabilidadesService service)
        {
            _service = service;
        }

        /// <summary>Busca habilidades na API do RBOSF pelo código.</summary>
        /// <param name="codigo">Código da habilidade no RBOSF (ex.: PROG, DOCK).</param>
        /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
        /// <returns>Habilidades encontradas no RBOSF.</returns>
        [HttpGet("rbosf/codigo/{codigo}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca habilidades no RBOSF pelo código", Description = "Consulta a API do RBOSF e retorna as habilidades correspondentes ao código informado, sem gravar no banco.")]
        [SwaggerResponse(200, "Habilidades retornadas com sucesso.", typeof(List<RbosfHabilidadeDto>))]
        [SwaggerResponse(400, "Código inválido ou erro ao consultar o RBOSF.")]
        public async Task<ActionResult<List<RbosfHabilidadeDto>>> BuscarNoRbosfPorCodigo(string codigo, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _service.BuscarNoRbosfPorCodigo(codigo, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Busca habilidades na API do RBOSF por termo/nome.</summary>
        /// <param name="termo">Texto a pesquisar (ex.: docker, python).</param>
        /// <param name="classificacao">Filtro opcional: hard ou soft.</param>
        /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
        /// <returns>Habilidades encontradas no RBOSF.</returns>
        [HttpGet("rbosf/termo/{termo}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca habilidades no RBOSF por termo", Description = "Consulta a API do RBOSF por nome/termo livre, com filtro opcional de classificação, sem gravar no banco.")]
        [SwaggerResponse(200, "Habilidades retornadas com sucesso.", typeof(List<RbosfHabilidadeDto>))]
        [SwaggerResponse(400, "Termo inválido ou erro ao consultar o RBOSF.")]
        public async Task<ActionResult<List<RbosfHabilidadeDto>>> BuscarNoRbosfPorTermo(
            string termo,
            [FromQuery] string? classificacao,
            CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _service.BuscarNoRbosfPorTermo(termo, classificacao, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Importa habilidades do RBOSF para o catálogo do NextHire.</summary>
        /// <param name="dto">Códigos das habilidades selecionadas.</param>
        /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
        /// <returns>Resumo da importação, incluindo códigos não encontrados.</returns>
        [HttpPost("rbosf/importar")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Importa habilidades do RBOSF", Description = "Busca cada código no RBOSF e faz upsert no catálogo local pelo código. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Importação concluída.", typeof(ResultadoImportacaoRbosfDto))]
        [SwaggerResponse(400, "Dados inválidos ou erro ao consultar o RBOSF.")]
        public async Task<ActionResult<ResultadoImportacaoRbosfDto>> ImportarDoRbosf(
            ImportarHabilidadesRbosfDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _service.ImportarDoRbosf(dto, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Lista todas as habilidades cadastradas.</summary>
        /// <returns>Lista completa de habilidades.</returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista todas as habilidades", Description = "Retorna todas as habilidades cadastradas no catálogo.")]
        [SwaggerResponse(200, "Habilidades listadas com sucesso.", typeof(List<ListarHabilidadeDto>))]
        [SwaggerResponse(400, "Erro ao listar as habilidades.")]
        public ActionResult<ListarHabilidadeDto> Listar()
        {
            try
            {
                var habilidade = _service.Listar();
                return Ok(habilidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Busca uma habilidade específica pelo ID.</summary>
        /// <param name="id">ID da habilidade desejada.</param>
        /// <returns>Dados da habilidade encontrada.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca uma habilidade pelo ID", Description = "Retorna os dados da habilidade correspondente ao ID informado.")]
        [SwaggerResponse(200, "Habilidade encontrada.", typeof(ListarHabilidadeDto))]
        [SwaggerResponse(400, "Habilidade não encontrada ou erro na busca.")]
        public ActionResult<ListarHabilidadeDto> ListarPorID(int id)
        {
            try
            {
                var habilidade = _service.ListarPorID(id);
                return Ok(habilidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Cadastra uma nova habilidade no catálogo.</summary>
        /// <param name="habilidade">Dados da habilidade a ser cadastrada.</param>
        /// <returns>Dados da habilidade criada.</returns>
        [HttpPost]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Cadastra uma nova habilidade", Description = "Cria uma habilidade no catálogo e retorna os dados da habilidade criada. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(201, "Habilidade cadastrada com sucesso.", typeof(ListarHabilidadeDto))]
        [SwaggerResponse(400, "Dados inválidos ou erro ao cadastrar a habilidade.")]
        public ActionResult Adicionar(AdicionarHabilidadeDto habilidade)
        {
            try
            {
               ListarHabilidadeDto habilidadedto = _service.Adicionar(habilidade);
                return StatusCode(201, habilidadedto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza os dados de uma habilidade existente.</summary>
        /// <param name="id">ID da habilidade a ser atualizada.</param>
        /// <param name="habilidade">Novos dados da habilidade.</param>
        /// <returns>Dados da habilidade atualizada.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Atualiza uma habilidade", Description = "Substitui os dados da habilidade informada e retorna os dados atualizados. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(204, "Habilidade atualizada com sucesso.", typeof(ListarHabilidadeDto))]
        [SwaggerResponse(400, "Dados inválidos, habilidade inexistente ou erro ao atualizar.")]
        public ActionResult Atualizar(int id, AdicionarHabilidadeDto habilidade)
        {
            try
            {
               ListarHabilidadeDto habilidadedto = _service.Atualizar(id, habilidade);
                return StatusCode(204, habilidadedto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove uma habilidade do catálogo.</summary>
        /// <param name="id">ID da habilidade a ser removida.</param>
        /// <returns>Resposta vazia com status 204.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Remove uma habilidade", Description = "Exclui definitivamente a habilidade informada do catálogo. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(204, "Habilidade removida com sucesso.")]
        [SwaggerResponse(400, "Habilidade não encontrada ou erro ao remover.")]
        public ActionResult Deletar(int id)
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

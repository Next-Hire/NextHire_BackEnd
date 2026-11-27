using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHireAPI.Applications.Services;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHire_API.DTOs.VagaDTO;

namespace NextHireAPI.Controllers
{
    /// <summary>Endpoints de gerenciamento de vagas de emprego.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VagaController : ControllerBase
    {
        private readonly VagaService _service;

        public VagaController(VagaService service) => _service = service;

        /// <summary>Lista todas as vagas cadastradas, ordenadas por importância (abertas primeiro, prioridade alta > media > baixa, abertura mais recente e fechamento como desempate).</summary>
        /// <returns>Lista completa de vagas com empresa, analista responsável e dados de remuneração.</returns>
        [HttpGet("vagas")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista todas as vagas", Description = "Retorna as vagas ordenadas por importância: status aberta, nível de prioridade, data de abertura e data de fechamento.")]
        [SwaggerResponse(200, "Vagas listadas com sucesso.", typeof(List<ListarVagaDto>))]
        [SwaggerResponse(404, "Nenhuma vaga cadastrada.")]
        public async Task<IActionResult> Listar()
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

        /// <summary>Lista as vagas de uma empresa específica, na mesma ordenação por importância.</summary>
        /// <param name="empresaId">ID da empresa cujas vagas serão listadas.</param>
        /// <returns>Vagas da empresa informada.</returns>
        [HttpGet("empresa/{empresaId}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista as vagas de uma empresa", Description = "Retorna as vagas vinculadas ao ID da empresa informado, ordenadas por importância.")]
        [SwaggerResponse(200, "Vagas listadas com sucesso.", typeof(List<ListarVagaDto>))]
        [SwaggerResponse(400, "ID de empresa inválido ou nenhuma vaga encontrada.")]
        public async Task<IActionResult> ListarPorEmpresaId(int empresaId)
        {
            try
            {
                return Ok(await _service.ListarPorEmpresaId(empresaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca vagas pelo setor (correspondência parcial, ignorando maiúsculas/minúsculas).</summary>
        /// <param name="setorId">ID do setor cujas vagas serão listadas.</param>
        /// <returns>Vagas do setor informado.</returns>
        [HttpGet("setor/{setorId}")]
        public async Task<IActionResult> ListarPorSetorId(int setorId)
        {
            try
            {
                return Ok(await _service.ListarPorSetorId(setorId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("titulo")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca vagas por título", Description = "Retorna as vagas cujo título contém o texto informado, ordenadas por importância.")]
        [SwaggerResponse(200, "Vagas listadas com sucesso.", typeof(List<ListarVagaDto>))]
        [SwaggerResponse(404, "Nenhuma vaga encontrada com o título informado.")]
        public async Task<IActionResult> ListarPorTitulo(string titulo)
        {
            try
            {
                return Ok(await _service.ListarPorTitulo(titulo));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Lista as vagas filtradas pelo status (abertas = true, fechadas = false).</summary>
        /// <param name="status">Status da vaga: true para abertas, false para fechadas.</param>
        /// <returns>Vagas no status informado.</returns>
        [HttpGet("status")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista vagas por status", Description = "Retorna as vagas com o status informado (abertas ou fechadas), ordenadas por importância.")]
        [SwaggerResponse(200, "Vagas listadas com sucesso.", typeof(List<ListarVagaDto>))]
        [SwaggerResponse(404, "Nenhuma vaga encontrada com o status informado.")]
        public async Task<IActionResult> ListarPorStatus(bool status)
        {
            try
            {
                return Ok(await _service.ListarPorStatus(status));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca uma vaga específica pelo ID.</summary>
        /// <param name="vagaId">ID da vaga desejada.</param>
        /// <returns>Dados completos da vaga.</returns>
        [HttpGet("{vagaId}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca uma vaga pelo ID", Description = "Retorna os dados completos de uma vaga específica.")]
        [SwaggerResponse(200, "Vaga encontrada.", typeof(ListarVagaDto))]
        [SwaggerResponse(404, "Vaga não encontrada.")]
        public async Task<IActionResult> BuscarPorId(int vagaId)
        {
            try
            {
                return Ok(await _service.BuscarPorId(vagaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Cadastra uma nova vaga no sistema.</summary>
        /// <param name="vaga">Dados da vaga: título, descrição, empresa, setor, analista responsável, remuneração, tipo da vaga, prioridade, idiomas e data de fechamento.</param>
        /// <returns>Mensagem de confirmação do cadastro.</returns>
        [HttpPost]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Cadastra uma nova vaga", Description = "Cria uma vaga vinculada a uma empresa e setor, com remuneração, tipo (nova_funcao, substituicao, expansao_equipe), prioridade (alta, media, baixa) e data de fechamento. Requer perfil de Analista de RH ou Adm.")]
        [SwaggerResponse(201, "Vaga cadastrada com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos: remuneração, tipo ou prioridade fora dos valores aceitos.")]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarVagaDto vaga)
        {
            try
            {
                await _service.Adicionar(vaga);

                return StatusCode(201, "Vaga cadastrada com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza os dados de uma vaga existente (atualização completa do corpo informado).</summary>
        /// <param name="vagaId">ID da vaga a ser atualizada.</param>
        /// <param name="vaga">Novos dados da vaga (todos os campos são substituídos).</param>
        /// <returns>Mensagem de confirmação da atualização.</returns>
        [HttpPatch("{vagaId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Atualiza uma vaga existente", Description = "Substitui os dados da vaga informada. Exige remuneração e tipo válidos. Requer perfil de Analista de RH ou Adm.")]
        [SwaggerResponse(204, "Vaga atualizada com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos, vaga inexistente ou nenhuma alteração detectada.")]
        public async Task<IActionResult> Atualizar(int vagaId, [FromBody] AtualizarVagaDto vaga)
        {
            try
            {
                await _service.Atualizar(vagaId, vaga);

                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove uma vaga do sistema.</summary>
        /// <param name="vagaId">ID da vaga a ser removida.</param>
        /// <returns>Resposta vazia com status 204.</returns>
        [HttpDelete("{vagaId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Remove uma vaga", Description = "Exclui definitivamente a vaga informada. Requer perfil de Analista de RH ou Adm.")]
        [SwaggerResponse(204, "Vaga removida com sucesso.")]
        [SwaggerResponse(400, "Vaga não encontrada ou erro ao remover.")]
        public async Task<IActionResult> Remover(int vagaId)
        {
            try
            {
                await _service.Remover(vagaId);

                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

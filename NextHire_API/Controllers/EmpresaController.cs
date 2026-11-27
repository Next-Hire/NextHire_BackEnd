using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.EmpresaDTOs;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de gerenciamento de empresas, incluindo listagem, busca, cadastro e atualização.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly EmpresaService _rep;

        public EmpresaController(EmpresaService rep) => _rep = rep;

        /// <summary>Lista todas as empresas cadastradas.</summary>
        /// <returns>Lista completa de empresas cadastradas.</returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista todas as empresas", Description = "Retorna todas as empresas cadastradas no sistema. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(200, "Empresas listadas com sucesso.", typeof(IEnumerable<LerEmpresaDTO>))]
        [SwaggerResponse(404, "Nenhuma empresa cadastrada.")]
        public async Task<ActionResult<IEnumerable<LerEmpresaDTO>>> Listar()
        {
            try
            {
                IEnumerable<LerEmpresaDTO?> lista_de_Empresas = await _rep.ListagemTotal();
                if (lista_de_Empresas == null || !lista_de_Empresas.Any())
                    return NotFound(lista_de_Empresas);
                return Ok(lista_de_Empresas);
            }

            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca uma empresa pelo nome.</summary>
        /// <param name="nome">Nome da empresa desejada.</param>
        /// <returns>Dados da empresa encontrada.</returns>
        [HttpGet("BuscarPorNome/{nome}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca uma empresa pelo nome", Description = "Retorna os dados da empresa com o nome informado. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(200, "Empresa encontrada.", typeof(LerEmpresaDTO))]
        [SwaggerResponse(404, "Empresa não encontrada.")]
        public async Task<ActionResult<LerEmpresaDTO>> BuscarPorNome(string nome)
        {
            try
            {
                LerEmpresaDTO Empresa = await _rep.ObterPorEmpresa(nome);
                return Ok(Empresa);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca uma empresa pelo ID.</summary>
        /// <param name="id">ID da empresa desejada.</param>
        /// <returns>Dados da empresa encontrada.</returns>
        [HttpGet("BuscarPorId/{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca uma empresa pelo ID", Description = "Retorna os dados da empresa com o ID informado. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(200, "Empresa encontrada.", typeof(LerEmpresaDTO))]
        [SwaggerResponse(404, "Empresa não encontrada.")]
        public async Task<ActionResult<LerEmpresaDTO>> BuscarPorId(int id)
        {
            try
            {
                LerEmpresaDTO Empresa = await _rep.ObterPorId(id);
                return Ok(Empresa);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Cadastra uma nova empresa no sistema.</summary>
        /// <param name="EmpresaCons">Dados da empresa a ser cadastrada.</param>
        /// <returns>Dados da empresa cadastrada.</returns>
        [HttpPost]
        [Authorize(Roles = "Adm")]
        [SwaggerOperation(Summary = "Cadastra uma nova empresa", Description = "Cria o cadastro de uma empresa no sistema e retorna os dados dela. Requer perfil de Adm.")]
        [SwaggerResponse(200, "Empresa cadastrada com sucesso.", typeof(CriarEmpresaDTO))]
        [SwaggerResponse(400, "Dados inválidos ou empresa já cadastrada.")]
        public async Task<ActionResult<CriarEmpresaDTO>> Adicionar([FromBody] CriarEmpresaDTO EmpresaCons)
        {
            try
            {
                CriarEmpresaDTO Empresa = await _rep.Adicionar(EmpresaCons);
                return Ok(Empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza o nome e/ou o status de uma empresa existente.</summary>
        /// <param name="id">ID da empresa a ser atualizada.</param>
        /// <param name="nome">Novo nome da empresa (opcional).</param>
        /// <param name="status">Novo status da empresa (opcional).</param>
        /// <returns>Dados da empresa atualizada.</returns>
        [HttpPut("Atualizar/{id}")]
        [Authorize(Roles = "Adm")]
        [SwaggerOperation(Summary = "Atualiza uma empresa existente", Description = "Atualiza o nome e/ou o status da empresa informada (parâmetros opcionais via query). Requer perfil de Adm.")]
        [SwaggerResponse(200, "Empresa atualizada com sucesso.", typeof(LerEmpresaDTO))]
        [SwaggerResponse(400, "Empresa não encontrada ou dados inválidos.")]
        public async Task<ActionResult<LerEmpresaDTO>> Atualizar(int id, string? nome, bool? status)
        {
            try
            {
                LerEmpresaDTO Empresa = await _rep.Atualizar(id, nome, status);
                return Ok(Empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove uma empresa do sistema.</summary>
        /// <param name="id">ID da empresa a ser removida.</param>
        /// <returns>Dados da empresa removida.</returns>
        [HttpDelete]
        [Authorize(Roles = "Adm")]
        [SwaggerOperation(Summary = "Remove uma empresa", Description = "Exclui definitivamente a empresa informada do sistema. Requer perfil de Adm.")]
        [SwaggerResponse(200, "Empresa removida com sucesso.", typeof(LerEmpresaDTO))]
        [SwaggerResponse(400, "Empresa não encontrada ou erro ao remover.")]
        public async Task<ActionResult<LerEmpresaDTO>> Remover(int id)
        {
            try
            {
                LerEmpresaDTO Empresa = await _rep.Remover(id);
                return Ok(Empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

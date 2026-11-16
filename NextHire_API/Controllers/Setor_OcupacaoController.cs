using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.Setor_OcupacaoDTOs;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de gerenciamento de setores de ocupação dentro das áreas de uma empresa.</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Setor_OcupacaoController : ControllerBase
    {
        private readonly Setor_OcupacaoService _service;
        public Setor_OcupacaoController(Setor_OcupacaoService service) => _service = service;

        /// <summary>Lista todos os setores de ocupação cadastrados.</summary>
        /// <returns>Lista completa de setores de ocupação.</returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista todos os setores de ocupação", Description = "Retorna todos os setores de ocupação cadastrados no sistema.")]
        [SwaggerResponse(200, "Setores listados com sucesso.", typeof(List<LerSetor_OcupacaoDTO2>))]
        [SwaggerResponse(404, "Nenhum setor de ocupação cadastrado.")]
        public async Task<ActionResult<IEnumerable<LerSetor_OcupacaoDTO2?>>> Listar()
        {
            try
            {
                IEnumerable<LerSetor_OcupacaoDTO2> listaSetores = await _service.Listar();
                if (listaSetores == null || !listaSetores.Any())
                    return NotFound(listaSetores);

                return Ok(listaSetores);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca um setor de ocupação específico pelo ID.</summary>
        /// <param name="setorId">ID do setor de ocupação desejado.</param>
        /// <returns>Dados do setor de ocupação encontrado.</returns>
        [HttpGet("BuscarPorId/{setorId}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca um setor de ocupação pelo ID", Description = "Retorna os dados do setor de ocupação correspondente ao ID informado.")]
        [SwaggerResponse(200, "Setor de ocupação encontrado.", typeof(LerSetor_OcupacaoDTO2))]
        [SwaggerResponse(404, "Setor de ocupação não encontrado.")]
        public async Task<ActionResult<LerSetor_OcupacaoDTO2?>> ObterSetorPorId(int setorId)
        {
            try
            {
                LerSetor_OcupacaoDTO2 setor = await _service.ObterSetorPorId(setorId);
                return Ok(setor);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca um setor de ocupação pelo nome.</summary>
        /// <param name="setorNome">Nome do setor de ocupação desejado.</param>
        /// <returns>Dados do setor de ocupação encontrado.</returns>
        [HttpGet("BuscarPorNome/{setorNome}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca um setor de ocupação pelo nome", Description = "Retorna os dados do setor de ocupação correspondente ao nome informado.")]
        [SwaggerResponse(200, "Setor de ocupação encontrado.", typeof(LerSetor_OcupacaoDTO2))]
        [SwaggerResponse(404, "Setor de ocupação não encontrado.")]
        public async Task<ActionResult<LerSetor_OcupacaoDTO2?>> ObterSetorNome(string setorNome)
        {
            try
            {
                LerSetor_OcupacaoDTO2 setor = await _service.ObterSetorNome(setorNome);
                return Ok(setor);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca o setor de ocupação de uma área, com filtro opcional pelo nome da área.</summary>
        /// <param name="areaId">ID da área de ocupação.</param>
        /// <param name="nomeArea">Nome opcional da área para refinar a busca.</param>
        /// <returns>Dados do setor de ocupação vinculado à área informada.</returns>
        [HttpGet("BuscarPorArea/{areaId}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Busca o setor de ocupação de uma área", Description = "Retorna o setor de ocupação vinculado à área informada, com filtro opcional pelo nome da área.")]
        [SwaggerResponse(200, "Setor de ocupação encontrado.", typeof(LerSetor_OcupacaoDTO2))]
        [SwaggerResponse(404, "Setor de ocupação não encontrado para a área informada.")]
        public async Task<ActionResult<LerSetor_OcupacaoDTO2?>> ObterSetorPorArea(int areaId, [FromQuery] string? nomeArea)
        {
            try
            {
                LerSetor_OcupacaoDTO2 setor = await _service.ObterSetorPorArea(areaId, nomeArea);
                return Ok(setor);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Lista os setores de ocupação de uma empresa filtrados por área.</summary>
        /// <param name="empresaId">ID da empresa.</param>
        /// <param name="empresaNome">Nome opcional da empresa para refinar a busca.</param>
        /// <param name="nomeArea">Nome opcional da área para refinar a busca.</param>
        /// <param name="areaId">ID da área de ocupação.</param>
        /// <returns>Setores de ocupação da empresa na área informada.</returns>
        [HttpGet("ListarPorEmpresaEArea")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista setores por empresa e área", Description = "Retorna os setores de ocupação da empresa informada, filtrados pelos dados de área recebidos por query.")]
        [SwaggerResponse(200, "Setores listados com sucesso.", typeof(List<LerSetor_OcupacaoDTO2>))]
        [SwaggerResponse(404, "Nenhum setor de ocupação encontrado para a empresa e área informadas.")]
        public async Task<ActionResult<IEnumerable<LerSetor_OcupacaoDTO2?>>> ObterListagemSetoresPorEmpresaEPorArea(
            [FromQuery] int empresaId, 
            [FromQuery] string? empresaNome, 
            [FromQuery] string? nomeArea, 
            [FromQuery] int areaId)
        {
            try
            {
                IEnumerable<LerSetor_OcupacaoDTO2?> listaSetores = await _service.ObterListagemSetoresPorEmpresaEPorArea(empresaId, empresaNome, nomeArea, areaId);
                return Ok(listaSetores);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Lista todos os setores de ocupação de uma empresa.</summary>
        /// <param name="empresaId">ID da empresa.</param>
        /// <param name="nomeEmpresa">Nome opcional da empresa para refinar a busca.</param>
        /// <returns>Setores de ocupação da empresa informada.</returns>
        [HttpGet("BuscarPorEmpresa")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lista os setores de ocupação de uma empresa", Description = "Retorna todos os setores de ocupação vinculados à empresa informada, com filtro opcional pelo nome da empresa.")]
        [SwaggerResponse(200, "Setores listados com sucesso.", typeof(List<LerSetor_OcupacaoDTO2>))]
        [SwaggerResponse(404, "Nenhum setor de ocupação encontrado para a empresa informada.")]
        public async Task<ActionResult<IEnumerable<LerSetor_OcupacaoDTO2?>>> ObterSetoresPorEmpresa(
            [FromQuery] int empresaId, 
            [FromQuery] string? nomeEmpresa)
        {
            try
            {
                IEnumerable<LerSetor_OcupacaoDTO2> listaSetores = await _service.ObterSetoresPorEmpresa(empresaId, nomeEmpresa);
                return Ok(listaSetores);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        /// <summary>Cadastra um novo setor de ocupação.</summary>
        /// <param name="criarDTO">Dados do setor de ocupação a ser criado.</param>
        /// <returns>Dados do setor de ocupação criado.</returns>
        [HttpPost]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Cadastra um novo setor de ocupação", Description = "Cria um setor de ocupação e retorna os dados do setor criado. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Setor de ocupação cadastrado com sucesso.", typeof(CriarSetor_OcupacaoDTO))]
        [SwaggerResponse(400, "Dados inválidos ou erro ao cadastrar o setor de ocupação.")]
        public async Task<ActionResult<CriarSetor_OcupacaoDTO>> Adicionar([FromBody] CriarSetor_OcupacaoDTO criarDTO)
        {
            try
            {
                CriarSetor_OcupacaoDTO setorCriado = await _service.Adicionar(criarDTO);
                return Ok(setorCriado);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza os dados de um setor de ocupação existente.</summary>
        /// <param name="setorId">ID do setor de ocupação a ser atualizado.</param>
        /// <param name="atualizarDTO">Novos dados do setor de ocupação.</param>
        /// <returns>Dados do setor de ocupação atualizado.</returns>
        [HttpPut("Atualizar/{setorId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Atualiza um setor de ocupação", Description = "Substitui os dados do setor de ocupação informado e retorna o setor atualizado. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Setor de ocupação atualizado com sucesso.", typeof(AtualizarSetor_OcupacaoDTO))]
        [SwaggerResponse(400, "Dados inválidos, setor inexistente ou erro ao atualizar.")]
        public async Task<ActionResult<AtualizarSetor_OcupacaoDTO>> Atualizar(int setorId, [FromBody] AtualizarSetor_OcupacaoDTO atualizarDTO)
        {
            try
            {
                var setorAtualizado = await _service.Atualizar(setorId, atualizarDTO);
                return Ok(setorAtualizado);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove um setor de ocupação de uma área de uma empresa.</summary>
        /// <param name="setorId">ID do setor de ocupação a ser removido.</param>
        /// <param name="areaId">ID da área à qual o setor pertence.</param>
        /// <param name="empresaId">ID da empresa à qual o setor pertence.</param>
        /// <param name="nomeEmpresa">Nome opcional da empresa para refinar a busca.</param>
        /// <param name="nomeArea">Nome opcional da área para refinar a busca.</param>
        /// <returns>Dados do setor de ocupação removido.</returns>
        [HttpDelete]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Remove um setor de ocupação", Description = "Exclui o setor de ocupação informado, identificado pelos dados de setor, área e empresa recebidos por query, e retorna o setor removido. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Setor de ocupação removido com sucesso.", typeof(LerSetor_OcupacaoDTO2))]
        [SwaggerResponse(400, "Setor de ocupação não encontrado ou erro ao remover.")]
        public async Task<ActionResult<LerSetor_OcupacaoDTO2>> RemoverSetor(
            int setorId, 
            [FromQuery] int areaId, 
            [FromQuery] int empresaId, 
            [FromQuery] string? nomeEmpresa, 
            [FromQuery] string? nomeArea)
        {
            try
            {
                LerSetor_OcupacaoDTO2? setorRemovido = await _service.RemoverSetor(setorId, areaId, empresaId, nomeEmpresa, nomeArea);
                return Ok(setorRemovido);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
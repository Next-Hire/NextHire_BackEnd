using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.InformacaoCandidatoDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de informações pessoais do candidato: nascimento, formação etc.</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InformacaoCandidatoController : ControllerBase
    {
        private readonly InformacaoCandidatoService _service;

        public InformacaoCandidatoController(InformacaoCandidatoService service)
        {
            _service = service;
        }

        /// <summary>Lista as informações pessoais de todos os candidatos cadastrados.</summary>
        /// <returns>Lista completa das informações de candidatos.</returns>
        [HttpGet]
        [Authorize(Roles = "Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista as informações de todos os candidatos", Description = "Retorna as informações pessoais de todos os candidatos cadastrados. Requer perfil de Gestor, Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Informações listadas com sucesso.", typeof(List<ListarInformacaoCandidatoDto>))]
        [SwaggerResponse(500, "Erro interno ao listar as informações.")]
        public IActionResult Listar()
        {
            try
            {
                return Ok(_service.Listar());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>Busca as informações pessoais de um candidato pelo ID do registro.</summary>
        /// <param name="id">ID do registro de informações desejado.</param>
        /// <returns>Dados das informações pessoais do registro informado.</returns>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Busca informações do candidato por ID", Description = "Retorna as informações pessoais vinculadas ao ID do registro informado. Requer perfil de Candidato, Gestor, Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Informações encontradas.", typeof(ListarInformacaoCandidatoDto))]
        [SwaggerResponse(404, "Registro de informações não encontrado.")]
        [SwaggerResponse(400, "ID ou dados do registro inválidos.")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                return Ok(_service.BuscarPorId(id));
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

        /// <summary>Busca as informações pessoais de um candidato pelo ID do usuário candidato.</summary>
        /// <param name="candidatoId">ID do usuário candidato cujas informações serão retornadas.</param>
        /// <returns>Dados das informações pessoais do candidato informado.</returns>
        [HttpGet("candidato/{candidatoId:int}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Busca informações pelo ID do candidato", Description = "Retorna as informações pessoais vinculadas ao ID do usuário candidato informado. Requer perfil de Candidato, Gestor, Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Informações encontradas.", typeof(ListarInformacaoCandidatoDto))]
        [SwaggerResponse(404, "Nenhuma informação encontrada para o candidato informado.")]
        [SwaggerResponse(400, "ID do candidato inválido.")]
        public IActionResult BuscarPorCandidato(int candidatoId)
        {
            try
            {
                return Ok(_service.BuscarPorCandidato(candidatoId));
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

        /// <summary>Adiciona as informações pessoais de um candidato.</summary>
        /// <param name="dto">Dados das informações: nascimento, formação e demais campos do registro.</param>
        /// <returns>Informações cadastradas, com a URL de consulta do registro criado.</returns>
        [HttpPost]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Adiciona informações do candidato", Description = "Cria um novo registro de informações pessoais para o candidato informado. Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(201, "Informações cadastradas com sucesso.", typeof(ListarInformacaoCandidatoDto))]
        [SwaggerResponse(400, "Dados inválidos para o cadastro das informações.")]
        public IActionResult Adicionar([FromBody] AdicionarInformacaoCandidatoDto dto)
        {
            try
            {
                var informacao = _service.Adicionar(dto);
                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = informacao.InfoCandidatoId },
                    informacao);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza as informações pessoais de um candidato.</summary>
        /// <param name="id">ID do registro de informações a ser atualizado.</param>
        /// <param name="dto">Novos dados das informações do candidato.</param>
        /// <returns>Informações atualizadas do registro.</returns>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Atualiza informações do candidato", Description = "Substitui os dados do registro de informações informado. Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(200, "Informações atualizadas com sucesso.", typeof(ListarInformacaoCandidatoDto))]
        [SwaggerResponse(404, "Registro de informações não encontrado.")]
        [SwaggerResponse(400, "Dados inválidos para a atualização.")]
        public IActionResult Atualizar(int id, [FromBody] AtualizarInformacaoCandidatoDto dto)
        {
            try
            {
                return Ok(_service.Atualizar(id, dto));
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

        /// <summary>Remove as informações pessoais de um candidato.</summary>
        /// <param name="id">ID do registro de informações a ser removido.</param>
        /// <returns>Resposta vazia com status 204.</returns>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Remove informações do candidato", Description = "Exclui definitivamente o registro de informações informado. Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(204, "Informações removidas com sucesso.")]
        [SwaggerResponse(404, "Registro de informações não encontrado.")]
        [SwaggerResponse(400, "ID inválido ou erro ao remover.")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _service.Deletar(id);
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

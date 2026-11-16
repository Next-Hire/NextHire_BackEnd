using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.ExperienciaProfissionalDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    /// <summary>Endpoints de experiências profissionais do candidato.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExperienciaProfissionalController : ControllerBase
    {
        private readonly ExperienciaProfissionalService _service;
        public ExperienciaProfissionalController(ExperienciaProfissionalService service) => _service = service;

        /// <summary>Lista as experiências profissionais de um usuário, com filtro opcional por status ativo.</summary>
        /// <param name="usuarioId">ID do usuário cujas experiências serão listadas.</param>
        /// <param name="ativo">Filtro opcional: true para apenas experiências ativas, false para apenas inativas (nulo lista todas).</param>
        /// <returns>Lista de experiências profissionais do usuário informado.</returns>
        [HttpGet]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista experiências profissionais por usuário", Description = "Retorna as experiências profissionais do usuário informado, podendo filtrar por status ativo/inativo. Requer perfil de Candidato, Gestor, Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Experiências listadas com sucesso.", typeof(List<ListarExperienciaProfissionalDto>))]
        [SwaggerResponse(404, "Usuário não encontrado ou nenhuma experiência registrada.")]
        public async Task<IActionResult> ListarExperienciasProfissionaisPorUsuario(int usuarioId, bool? ativo)
        {
            try
            {
                return Ok(await _service.ListarExperienciaProfissionalPorUsuario(usuarioId, ativo));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca uma experiência profissional pelo ID.</summary>
        /// <param name="experienciaProfissionalId">ID da experiência profissional desejada.</param>
        /// <returns>Dados completos da experiência profissional.</returns>
        [HttpGet("{experienciaProfissionalId}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Busca uma experiência profissional pelo ID", Description = "Retorna os dados completos da experiência profissional informada. Requer perfil de Candidato, Gestor, Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Experiência profissional encontrada.", typeof(ListarExperienciaProfissionalDto))]
        [SwaggerResponse(404, "Experiência profissional não encontrada.")]
        public async Task<IActionResult> BuscarExperienciaProfissional(int experienciaProfissionalId)
        {
            try
            {
                return Ok(await _service.BuscarExperienciaProfissional(experienciaProfissionalId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Cadastra uma nova experiência profissional.</summary>
        /// <param name="experienciaProfissionalDto">Dados da experiência: cargo, empresa, período, atividades e demais campos do registro.</param>
        /// <returns>Mensagem de confirmação do cadastro.</returns>
        [HttpPost]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Cadastra uma experiência profissional", Description = "Cria um novo registro de experiência profissional para o candidato. Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(201, "Experiência profissional cadastrada com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos para o cadastro da experiência.")]
        public async Task<IActionResult> CadastrarExperienciaProfissional([FromBody] AdicionarExperienciaProfissionalDto experienciaProfissionalDto)
        {
            try
            {
                await _service.CadastrarExperienciaProfissional(experienciaProfissionalDto);
                return StatusCode(201, "Experiência profissional cadastrada com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza uma experiência profissional existente.</summary>
        /// <param name="experienciaProfissionalId">ID da experiência profissional a ser atualizada.</param>
        /// <param name="experienciaProfissionalDto">Novos dados da experiência profissional.</param>
        /// <returns>Resposta vazia com status 204.</returns>
        [HttpPut("{experienciaProfissionalId}")]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Atualiza uma experiência profissional", Description = "Substitui os dados da experiência profissional informada. Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(204, "Experiência profissional atualizada com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos ou experiência não encontrada.")]
        public async Task<IActionResult> AtualizarExperienciaProfissional(int experienciaProfissionalId, [FromBody] AtualizarExperienciaProfissionalDto experienciaProfissionalDto)
        {
            try
            {
                await _service.AtualizarExperienciaProfissional(experienciaProfissionalId, experienciaProfissionalDto);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove uma experiência profissional.</summary>
        /// <param name="experienciaId">ID da experiência profissional a ser removida.</param>
        /// <returns>Status 204 acompanhado de mensagem de confirmação.</returns>
        [HttpDelete]
        [Authorize(Roles = "Candidato,Adm")]
        [SwaggerOperation(Summary = "Remove uma experiência profissional", Description = "Exclui a experiência profissional informada (ID recebido por query string). Requer perfil de Candidato ou Administrador.")]
        [SwaggerResponse(204, "Experiência removida com sucesso.")]
        [SwaggerResponse(400, "Experiência não encontrada ou erro ao remover.")]
        public async Task<IActionResult> RemoverExperienciaProfissional(int experienciaId)
        {
            try
            {
                await _service.RemoverExperienciaProfissional(experienciaId);
                return StatusCode(204, "Experiencia removida com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

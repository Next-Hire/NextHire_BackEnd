
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using NextHireAPI.Applications.Services;
using NextHireAPI.DTOs.UsuarioDto;
using NextHire_API.Exceptions;
using System.Net.Mail;
using NextHireAPI.Domains;
using NextHireApi.DTOs.UsuarioDTO;

namespace NextHireAPI.Controllers
{
    /// <summary>Endpoints de gerenciamento de usuários (funcionários e candidatos), incluindo cadastro, atualização, recuperação de senha e remoção.</summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        // Armazena temporariamente os códigos enviados por e-mail.
        private static readonly ConcurrentDictionary<string, string> CodigosVerificacao = new();

        public UsuarioController(UsuarioService service) => _service = service;

        /// <summary>Lista todos os usuários cadastrados (candidatos e funcionários).</summary>
        /// <returns>Lista completa de usuários cadastrados.</returns>
        [HttpGet("usuarios")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista todos os usuários", Description = "Retorna todos os usuários cadastrados no sistema, tanto candidatos quanto funcionários. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Usuários listados com sucesso.", typeof(List<ListarUsuariosDto>))]
        [SwaggerResponse(404, "Nenhum usuário cadastrado.")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                return Ok(await _service.ListarUsuarios());
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Lista os funcionários vinculados a uma empresa específica.</summary>
        /// <param name="empresaId">ID da empresa cujos funcionários serão listados.</param>
        /// <returns>Lista de funcionários da empresa informada.</returns>
        [HttpGet("funcionarios")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista os funcionários de uma empresa", Description = "Retorna os funcionários vinculados ao ID da empresa informado. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Funcionários listados com sucesso.", typeof(List<ListarFuncionarioDto>))]
        [SwaggerResponse(404, "Nenhum funcionário encontrado para a empresa informada.")]
        public async Task<IActionResult> ListarFuncionariosPorEmpresa(int empresaId)
        {
            try
            {
                return Ok(await _service.ListarFuncionariosPorEmpresa(empresaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Lista todos os candidatos cadastrados.</summary>
        /// <returns>Lista completa de candidatos.</returns>
        [HttpGet("candidatos")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Lista todos os candidatos", Description = "Retorna todos os candidatos cadastrados no sistema. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Candidatos listados com sucesso.", typeof(List<ListarCandidatoDto>))]
        [SwaggerResponse(404, "Nenhum candidato cadastrado.")]
        public async Task<IActionResult> ListarCandidatos()
        {
            try
            {
                return Ok(await _service.ListarCandidatos());
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca um funcionário específico pelo ID.</summary>
        /// <param name="funcionarioId">ID do funcionário desejado.</param>
        /// <returns>Dados do funcionário.</returns>
        [HttpGet("funcionario/{funcionarioId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Busca um funcionário pelo ID", Description = "Retorna os dados de um funcionário específico. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Funcionário encontrado.", typeof(ListarFuncionarioDto))]
        [SwaggerResponse(404, "Funcionário não encontrado.")]
        public async Task<IActionResult> BuscarFuncionarioPorId(int funcionarioId)
        {
            try
            {
                return Ok(await _service.BuscarFuncionarioPorId(funcionarioId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Busca o perfil completo de um candidato específico pelo ID.</summary>
        /// <param name="candidatoId">ID do candidato desejado.</param>
        /// <returns>Dados completos do perfil do candidato.</returns>
        [HttpGet("candidato/{candidatoId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Busca o perfil completo de um candidato pelo ID", Description = "Retorna o perfil completo de um candidato específico. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Candidato encontrado.", typeof(ListarCandidatoDto))]
        [SwaggerResponse(404, "Candidato não encontrado.")]
        public async Task<IActionResult> BuscarCandidatoPorId(int candidatoId)
        {
            try
            {
                return Ok(await _service.BuscarCandidatoPorId(candidatoId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Cadastra uma lista de funcionários vinculados a uma empresa específica.</summary>
        /// <param name="empresaId">ID da empresa à qual os funcionários serão vinculados.</param>
        /// <param name="funcionarios">Lista de dados dos funcionários a serem cadastrados.</param>
        /// <returns>Mensagem de confirmação do cadastro.</returns>
        [HttpPost("funcionarios/{empresaId}")]
        [Authorize(Roles = "AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Cadastra funcionários de uma empresa", Description = "Cria um lote de funcionários vinculados ao ID da empresa informado. Requer perfil de Analista de RH ou Administrador.")]
        [SwaggerResponse(200, "Funcionários cadastrados com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos ou empresa inexistente.")]
        public async Task<IActionResult> AdicionarFuncionarios(int empresaId, [FromBody] List<AdicionarFuncionarioDto> funcionarios)
        {
            try
            {
                await _service.AdicionarFuncionarios(empresaId, funcionarios);
                return Ok("Funcionários cadastrados com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Cadastro público de um candidato no sistema.</summary>
        /// <param name="candidato">Dados do candidato a ser cadastrado.</param>
        /// <returns>Mensagem de confirmação do cadastro.</returns>
        [HttpPost("candidato")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Cadastra um candidato", Description = "Cria o cadastro de um candidato no sistema. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(201, "Candidato cadastrado com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos: e-mail, CPF ou outros campos fora das regras de negócio.")]
        public async Task<IActionResult> CadastroCandidato(
            [FromBody] CadastrarCandidatoDto candidato)
        {
            try
            {
                await _service.CadastroCandidato(candidato);

                return StatusCode(201, "Candidato cadastrado com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza os dados gerais de um usuário (candidato ou funcionário).</summary>
        /// <param name="usuarioId">ID do usuário a ser atualizado.</param>
        /// <param name="usuarioAtualizado">Novos dados do usuário.</param>
        /// <returns>Resposta vazia com status 204.</returns>
        [HttpPatch("{usuarioId}")]
        [Authorize(Roles = "Candidato,Gestor,AnalistaRh,Adm")]
        [SwaggerOperation(Summary = "Atualiza os dados gerais de um usuário", Description = "Atualiza os dados gerais (candidato ou funcionário) do usuário informado. Requer qualquer perfil autenticado (Candidato, Gestor, Analista de RH ou Administrador).")]
        [SwaggerResponse(204, "Usuário atualizado com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos, usuário inexistente ou nenhuma alteração detectada.")]
        public async Task<IActionResult> AtualizarUsuarioGeral(
            int usuarioId,
            [FromBody] AtualizarUsuarioDto usuarioAtualizado)
        {
            try
            {
                await _service.AtualizarUsuarioGeral(usuarioId, usuarioAtualizado);

                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Envia um código de verificação por e-mail para recuperação de senha.</summary>
        /// <param name="email">E-mail do usuário que receberá o código de verificação.</param>
        /// <returns>Mensagem de confirmação do envio.</returns>
        [HttpPost("enviar-codigo")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Envia código de recuperação de senha", Description = "Envia um código de verificação para o e-mail informado, utilizado na recuperação de senha. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(200, "Código de verificação enviado para o e-mail.")]
        [SwaggerResponse(400, "Formato de e-mail inválido.")]
        public async Task<IActionResult> EnviarCodigoVerificacaoVerificacao(string email)
        {
            try
            {
                await _service.EnviarCodigo(email);
                return Ok("Código de verificação enviado para o e-mail.");
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Redefine a senha do usuário utilizando o código de verificação recebido por e-mail.</summary>
        /// <param name="dados">E-mail, nova senha e código de verificação recebido.</param>
        /// <returns>Resposta vazia com status 204.</returns>
        [HttpPut("senha")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Redefine a senha do usuário", Description = "Atualiza a senha do usuário mediante validação do código de verificação enviado por e-mail. Endpoint público, não exige autenticação.")]
        [SwaggerResponse(204, "Senha atualizada com sucesso.")]
        [SwaggerResponse(400, "E-mail inexistente, código de verificação inválido ou expirado.")]
        public async Task<IActionResult> AtualizarSenha([FromBody] AtualizarSenhaDto dados)
        {
            try
            {
                await _service.AtualizarSenha(dados.Email, dados.Senha, dados.CodigoVerificacao);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove um usuário do sistema.</summary>
        /// <param name="usuarioId">ID do usuário a ser removido.</param>
        /// <returns>Mensagem de confirmação da remoção.</returns>
        [HttpDelete("{usuarioId}")]
        [Authorize(Roles = "Adm")]
        [SwaggerOperation(Summary = "Remove um usuário", Description = "Exclui definitivamente o usuário informado do sistema. Requer perfil de Administrador.")]
        [SwaggerResponse(204, "Usuário removido com sucesso.")]
        [SwaggerResponse(400, "Usuário não encontrado ou erro ao remover.")]
        public async Task<IActionResult> RemoverUsuario(int usuarioId)
        {
            try
            {
                await _service.RemoverUsuario(usuarioId);

                return StatusCode(204, "Usuário removido com sucesso.");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

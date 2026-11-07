
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using NextHireAPI.Applications.Services;
using NextHireAPI.DTOs.UsuarioDto;
using NextHire_API.Exceptions;
using System.Net.Mail;
using NextHireAPI.Domains;
using NextHireApi.DTOs.UsuarioDTO;

namespace NextHireAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        // Armazena temporariamente os códigos enviados por e-mail.
        private static readonly ConcurrentDictionary<string, string> CodigosVerificacao = new();

        public UsuarioController(UsuarioService service) => _service = service;

        [HttpGet("usuarios")]
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

        [HttpGet("funcionarios")]
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

        [HttpGet("candidatos")]
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

        [HttpGet("funcionario/{funcionarioId}")]
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

        [HttpGet("candidato/{candidatoId}")]
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

        [HttpPost("funcionarios/{empresaId}")]
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

        [HttpPost("candidato")]
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

        [HttpPatch("{usuarioId}")]
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

        [HttpPost("enviar-codigo")]
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

        [HttpPut("/senha")]
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

        [HttpDelete("{usuarioId}")]
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

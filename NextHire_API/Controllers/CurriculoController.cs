using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.S3;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculoController : ControllerBase
    {
        private readonly CurriculoService _curriculoService;
        private readonly GeminiCurriculoService _geminiCurriculo;

        public CurriculoController(CurriculoService service, GeminiCurriculoService geminiCurriculo)
        {
            _curriculoService = service;
            _geminiCurriculo = geminiCurriculo;
        }

        [HttpGet]
        public async Task<ActionResult> ObterCurriculo(string nomeObjeto)
        {
            try
            {
                string link = await _curriculoService.GerarLinkVisualizacaoCurriculo(nomeObjeto);
                return Ok(link);
            }

            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("parse")]
        public async Task<IActionResult> ParseCurriculo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            var tiposPermitidos = new[] { "application/pdf", "image/jpeg", "image/png" };
            if (!tiposPermitidos.Contains(file.ContentType))
                return BadRequest("Formato inválido. Envie PDF, JPG ou PNG.");

            try
            {
                var resultado = await _geminiCurriculo.ExtrairDadosAsync(file);
                return Ok(resultado);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}

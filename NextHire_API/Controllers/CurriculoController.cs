using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.S3;
using NextHire_API.DTOs.CurriculoAWS;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculoController : ControllerBase
    {
        private readonly CurriculoService _curriculoService;
        private readonly GeminiCurriculoService _geminiCurriculo;
        private readonly GroqCurriculoService _groqService;

        public CurriculoController(CurriculoService service, GeminiCurriculoService geminiCurriculo, GroqCurriculoService groqService)
        {
            _curriculoService = service;
            _geminiCurriculo = geminiCurriculo;
            _groqService = groqService;
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

        [HttpPost("curriculo/url/")]
        public async Task<ActionResult<CurriculoDTO?>> ConverterCurriculoPorUrl(string url)
        {
            try
            {
            CurriculoDTO? response = await _geminiCurriculo.ExtrairDadosPorUrlAsync(url);
            return Ok(response);
            }
            catch (DomainException e)
            {
                return BadRequest(e.Message);
            }

            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost("curriculo/file/")]
        public async Task<ActionResult<CurriculoDTO?>> ConverterCurriculoPorArquivo(IFormFile file)
        {
            try
            {
                var resultado = await _geminiCurriculo.ExtrairCurriculoArquivoAsync(file);
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
        
        
        [HttpPost("curriculo/file/groq")]
        public async Task<ActionResult<CurriculoDTO?>> ConverterCurriculoPorArquivo2(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo foi enviado.");

            try
            {
                CurriculoDTO? resultado = await _groqService.ExtrairCurriculoArquivoAsync(file);
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
        
        
        [HttpPost("upload")]
        public async Task<ActionResult<string>> UploadCurriculo(IFormFile file)
        {

            try
            {
                var s3Key = await _curriculoService.UploadCurriculoAsync(file);
                return Ok(s3Key);
            }
            catch (DomainException e)
            {
                return BadRequest(e.Message);

            }
        }

    }
}

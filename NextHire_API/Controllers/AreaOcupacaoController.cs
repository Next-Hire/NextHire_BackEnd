using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.AreaOcupacaoDto;
using System.Runtime.ConstrainedExecution;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaOcupacaoController : ControllerBase
    {
        private readonly AreaOcupacaoService _service;

        public AreaOcupacaoController(AreaOcupacaoService areaOcupacaoService)
        {
            _service = areaOcupacaoService;
        }

        [HttpGet]
        public ActionResult Listar()
        {
            try
            {
                var areas = _service.Listar();
                return Ok(areas);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult ListarPorId(int id)
        {
            try
            {
                var areaOcupacao = _service.ListarPorID(id);
                if (areaOcupacao == null)
                {
                    return NotFound();
                }
                return Ok(areaOcupacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("nome/{nome}")]
        public ActionResult ListarPorNome(string nome)
        {
            try
            {
                var areaOcupacao = _service.ListarPorNome(nome);
                if (areaOcupacao == null)
                {
                    return NotFound();
                }
                return Ok(areaOcupacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar(AdicionarAreaOcupacao areaOcupacao)
        {
            try
            {
                ListarAreaOcupacaoDto area = _service.Adicionar(areaOcupacao);  
                return StatusCode(201, area);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(int id, AdicionarAreaOcupacao areaOcupacao)
        {
            try
            {
                var area = _service.Atualizar(id, areaOcupacao);
                return StatusCode(204, area);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Excluir(int id)
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
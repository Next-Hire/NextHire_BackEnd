using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.AnotacaoDto;
using Microsoft.AspNetCore.Mvc;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnotacaoController : ControllerBase
    {
        private readonly AnotacaoService _service;

        public AnotacaoController(AnotacaoService anotacaoService)
        {
            _service = anotacaoService;
        }

        [HttpGet]
        public ActionResult Listar()
        {
            try
            {
                var anotacoes = _service.Listar();
                return Ok(anotacoes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult BuscarPorId(int id)
        {
            try
            {
                var anotacao = _service.BuscarPorId(id);
                return Ok(anotacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar(AdicionarAnotacaoDto anotacaoDto)
        {
            try
            {
                var anotacao = _service.Adicionar(anotacaoDto);
                return Ok(anotacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(AdicionarAnotacaoDto anotacaoDto, int id)
        {
            try
            {
                var anotacao = _service.Atualizar(anotacaoDto, id);
                return Ok(anotacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Deletar(int id)
        {
            try
            {
                _service.Deletar(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

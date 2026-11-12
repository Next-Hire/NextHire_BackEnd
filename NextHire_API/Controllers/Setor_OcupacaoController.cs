using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.Setor_OcupacaoDTOs; 
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Setor_OcupacaoController : ControllerBase
    {
        private readonly Setor_OcupacaoService _service;
        public Setor_OcupacaoController(Setor_OcupacaoService service) => _service = service;

        [HttpGet]
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

        [HttpGet("BuscarPorId/{setorId}")]
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

        [HttpGet("BuscarPorNome/{setorNome}")]
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

        [HttpGet("BuscarPorArea/{areaId}")]
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

        [HttpGet("ListarPorEmpresaEArea")]
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

        [HttpGet("BuscarPorEmpresa")]
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
        
        [HttpPost]
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

        [HttpPut("Atualizar/{setorId}")]
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

        [HttpDelete]
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
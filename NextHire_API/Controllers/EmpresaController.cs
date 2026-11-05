using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.EmpresaDTOs;
using NextHire_API.Exceptions;

namespace NextHire_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly EmpresaService _rep;

        public EmpresaController(EmpresaService rep) => _rep = rep;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LerEmpresaDTO>>> Listar()
        {
            try
            {
                IEnumerable<LerEmpresaDTO?> lista_de_empresas = await _rep.ListagemTotal();
                if (lista_de_empresas == null || !lista_de_empresas.Any())
                    return NotFound(lista_de_empresas);
                return Ok(lista_de_empresas);
            }

            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorNome/{nome}")]
        public async Task<ActionResult<LerEmpresaDTO>> BuscarPorNome(string nome)
        {
            try
            {
                LerEmpresaDTO empresa = await _rep.ObterPorEmpresa(nome);
                return Ok(empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<LerEmpresaDTO>> BuscarPorId(int id)
        {
            try
            {
                LerEmpresaDTO empresa = await _rep.ObterPorId(id);
                return Ok(empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CriarEmpresaDTO>> Adicionar([FromBody] CriarEmpresaDTO empresaCons)
        {
            try
            {
                CriarEmpresaDTO empresa = await _rep.Adicionar(empresaCons);
                return Ok(empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar/{id}")]
        public async Task<ActionResult<LerEmpresaDTO>> Atualizar(int id, [FromBody] LerEmpresaDTO? empresaCons)
        {
            try
            {
                LerEmpresaDTO empresa = await _rep.Atualizar(id, empresaCons);
                return Ok(empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<LerEmpresaDTO>> Remover(int id)
        {
            try
            {
                LerEmpresaDTO empresa = await _rep.Remover(id);
                return Ok(empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

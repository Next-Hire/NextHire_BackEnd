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
                IEnumerable<LerEmpresaDTO?> lista_de_Empresas = await _rep.ListagemTotal();
                if (lista_de_Empresas == null || !lista_de_Empresas.Any())
                    return NotFound(lista_de_Empresas);
                return Ok(lista_de_Empresas);
            }

            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("BuscarPorNome/{nome}")]
        public async Task<ActionResult<LerEmpresaDTO>> BuscarPorNome(string nome)
        {
            try
            {
                LerEmpresaDTO Empresa = await _rep.ObterPorEmpresa(nome);
                return Ok(Empresa);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<LerEmpresaDTO>> BuscarPorId(int id)
        {
            try
            {
                LerEmpresaDTO Empresa = await _rep.ObterPorId(id);
                return Ok(Empresa);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CriarEmpresaDTO>> Adicionar([FromBody] CriarEmpresaDTO EmpresaCons)
        {
            try
            {
                CriarEmpresaDTO Empresa = await _rep.Adicionar(EmpresaCons);
                return Ok(Empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar/{id}")]
        public async Task<ActionResult<LerEmpresaDTO>> Atualizar(int id, string? nome, bool? status)
        {
            try
            {
                LerEmpresaDTO Empresa = await _rep.Atualizar(id, nome, status);
                return Ok(Empresa);
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
                LerEmpresaDTO Empresa = await _rep.Remover(id);
                return Ok(Empresa);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

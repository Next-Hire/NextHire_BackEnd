using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHire_API.Applications.Services;
using NextHire_API.DTOs.ProcessoSeletivoDTO;
using NextHire_API.Exceptions;
using NextHire_API.Applications.Pagination;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.InteropServices;

namespace NextHire_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProcessoSeletivoController : ControllerBase
    {
        private readonly ProcessoSeletivoService _service;

        public ProcessoSeletivoController(ProcessoSeletivoService service)
            => _service = service;

        [HttpGet]
        [Authorize(Roles="AnalistaRh,Gestor")]
        [SwaggerOperation(Summary = "Listar todos os processos seletivos", Description = "Retorna todos os processos seletivos cadastradas no sistema. Requer perfil de Gestor/AnalistaRH.")]
        [SwaggerResponse(200, "Processos seletivos listados com sucesso.", typeof(List<ListarProcessoSeletivoDto>))]
        [SwaggerResponse(400, "Erro ao listar os processos seletivos.")]
        public async Task<ActionResult<ListarProcessoSeletivoDto>> ListarProcessosSeletivos([FromQuery] int? pagina, [FromQuery] int? tamanhoPagina)
        {
            try
            {
                List<ListarProcessoSeletivoDto> processos = await _service.ListarProcessosSeletivos();
                return Ok(processos.Paginar(pagina, tamanhoPagina));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("Empresa/{empresaId}")]
        [Authorize(Roles="AnalistaRh,Gestor")]
        [SwaggerOperation(Summary = "Listar todos os processos seletivos de uma empresa", Description = "Retorna todos os processos seletivos da empresa cadastrados no sistema. Requer perfil de Gestor/AnalistaRH.")]
        [SwaggerResponse(200, "Processos seletivos listados com sucesso.", typeof(List<ListarProcessoSeletivoDto>))]
        [SwaggerResponse(400, "Erro ao listar os processos seletivos.")]
        public async Task<ActionResult<ListarProcessoSeletivoDto>> ListarProcessosSeletivosPorEmpresaId(int empresaId)
        {
            try
            {
                return Ok(await _service.ListarProcessosSeletivosPorEmpresaId(empresaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Empresa/Setor/{setorId}")]
        [Authorize(Roles="AnalistaRh,Gestor")]
        [SwaggerOperation(Summary = "Listar todos os processos seletivos por setor dentro de uma empresa", Description = "Retorna todos os processos seletivos do setor de uma empresa cadastrados no sistema. Requer perfil de Gestor/AnalistaRH.")]
        [SwaggerResponse(200, "Processos seletivos listados com sucesso.", typeof(List<ListarProcessoSeletivoDto>))]
        [SwaggerResponse(400, "Erro ao listar os processos seletivos.")]
        public async Task<ActionResult<ListarProcessoSeletivoDto>> ListarProcessosSeletivosPorSetorId(int setorId)
        {
            try
            {
                return Ok(await _service.ListarProcessosSeletivosPorSetorId(setorId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Empresa/Area/{areaId}")]
        [Authorize(Roles="AnalistaRh,Gestor")]
        [SwaggerOperation(Summary = "Listar todos os processos seletivos por area dentro de uma empresa", Description = "Retorna todos os processos seletivos do area de uma empresa cadastrados no sistema. Requer perfil de Gestor/AnalistaRH.")]
        [SwaggerResponse(200, "Processos seletivos listados com sucesso.", typeof(List<ListarProcessoSeletivoDto>))]
        [SwaggerResponse(400, "Erro ao listar os processos seletivos.")]
        public async Task<ActionResult<ListarProcessoSeletivoDto>> ListarProcessosSeletivosPorAreaId(int areaId)
        {
            try
            {
                return Ok(await _service.ListarProcessosSeletivosPorAreaId(areaId));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpGet("ProcessoSeletivo/{processoId}")]
        [Authorize(Roles="AnalistaRh,Gestor,Candidato")]
        [SwaggerOperation(Summary = "Buscar um processo seletivo por ID dentro do sistema", Description = "Retorna o processo seletivo com o mesmo ID buscado se cadastrado no sistema. Requer perfil de Gestor/AnalistaRH.")]
        [SwaggerResponse(200, "Processos seletivo encontrado com sucesso.", typeof(List<ListarProcessoSeletivoDto>))]
        [SwaggerResponse(400, "Erro ao buscar o processos seletivo.")]
        public async Task<ActionResult<ListarProcessoSeletivoDto>> BuscarProcessoSeletivoPorId(int processoId)
        {
            try
            {
                return Ok(await _service.BuscarProcessoSeletivoPorId(processoId));
            }
            catch (DomainException ex)
            {
                return StatusCode(404, $"Erro ao buscar o processos seletivo.\nERRO: {ex.Message}" );
            }
        }
        
        [HttpPost]
        [Authorize(Roles="AnalistaRh")]
        [SwaggerOperation(Summary = "Adicionar um processo seletivo ao sistema", Description = "Possibilita Cadastrar um processo seletivo no sistema. Requer perfil de Gestor/AnalistaRH.")]
        [SwaggerResponse(200, "Processos seletivo encontrado com sucesso.", typeof(List<ListarProcessoSeletivoDto>))]
        [SwaggerResponse(400, "Erro ao cadastrar o processos seletivo.")]
        public async Task<ActionResult> AdiconarProcessoSeletivo(AdicionarProcessoSeletivoDto processoDto)
        {
            try
            {
                await _service.AdicionarProcessoSeletivo(processoDto);
                return Created("Processo criado com sucesso", processoDto);
            }
            catch (DomainException ex)
            {
                return StatusCode(404, $"Erro ao cadastrar o processos seletivo.\nERRO: {ex.Message}" );
            }
        }

        [HttpPatch("ProcessoSeletivo/Atualizar/{processoId}")]
        [Authorize(Roles="AnalistaRh")]
        [SwaggerOperation(Summary = "Atualizar um processo seletivo existente no sistema", Description = "Possibilita atualizar um processo seletivo do sistema. Requer perfil de Gestor/AnalistaRH.")]
        [SwaggerResponse(200, "Processos seletivo encontrado com sucesso.", typeof(List<ListarProcessoSeletivoDto>))]
        [SwaggerResponse(400, "Erro ao cadastrar o processos seletivo.")]
        public async Task<ActionResult> AtualizarProcessoSeletivo(int processoId, AtualizarProcessoSeletivoDto processoDto)
        {
            try
            {
                await _service.AtualizarProcessoSeletivo(processoId, processoDto.TituloProcesso, processoDto.DataFechamento);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return StatusCode(404, $"Erro ao atualizar o processos seletivo.\nERRO: {ex.Message}" );
            }
        }

        [HttpPatch("ProcessoSeletivo/Remover/{processoId}")]
        [Authorize(Roles="AnalistaRh")]
        [SwaggerOperation(Summary = "Remover um processo seletivo existente no sistema", Description = "Possibilita remover um processo seletivo do sistema. Requer perfil de Gestor/AnalistaRH.")]
        [SwaggerResponse(200, "Processos seletivo encontrado com sucesso.", typeof(List<ListarProcessoSeletivoDto>))]
        [SwaggerResponse(400, "Erro ao cadastrar o processos seletivo.")]
        public async Task<ActionResult> Remover(int processoId)
        {
            try
            {
                await _service.RemoverProcessoSeletivo(processoId);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return StatusCode(404, $"Erro ao atualizar o processos seletivo.\nERRO: {ex.Message}" );
            }
        }

        [HttpGet("ProcessoSeletivo/status/{status}")]
        [SwaggerOperation(Summary = "Listar todos os processos seletivos por status", Description = "Retorna todos os processos seletivos por status cadastradas no sistema. Requer perfil de Gestor/AnalistaRH.")]
        [SwaggerResponse(200, "Processos seletivos listados com sucesso.", typeof(List<ListarProcessoSeletivoDto>))]
        [SwaggerResponse(400, "Erro ao listar os processos seletivos.")]
        public async Task<ActionResult> ListarPorStatus(bool status)
        {
            try
            {
                return Ok(await _service.ListarProcessosSeletivosPorStatus(status));
            } 
            catch (DomainException ex)
            {
                return StatusCode(404, $"Erro ao buscar o processos seletivo.\nERRO: {ex.Message}");
            }
        }
    }
}
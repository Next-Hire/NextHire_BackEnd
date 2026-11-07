using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHireAPI.Applications.Services;
using NextHireAPI.DTOs.Autenticacao;

namespace NextHire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly AutenticacaoService _service;
        public AutenticacaoController(AutenticacaoService service)
            => _service = service;

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                return Ok(await _service.Login(loginDto.email, loginDto.senha));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

            }
        }
    }
}
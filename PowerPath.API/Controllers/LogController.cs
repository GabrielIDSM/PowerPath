using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LogController(ILogApplicationService logAppService) : ControllerBase
    {
        private readonly ILogApplicationService _logAppService = logAppService;

        [HttpGet("{ano}/{mes}/{dia}")]
        public IActionResult Listar(int ano, int mes, int dia)
        {
            Resposta<List<LogDTO>> rLogs = _logAppService.ListarPorData(ano, mes, dia);

            if (rLogs.IsSucesso)
            {
                return Ok(rLogs.Resultado);
            }
            else
            {
                return BadRequest(rLogs.Mensagem);
            }
        }
    }
}

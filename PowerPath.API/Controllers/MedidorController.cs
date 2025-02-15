using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MedidorController(IMedidorApplicationService medidorAppService) : ControllerBase
    {
        private readonly IMedidorApplicationService _medidorAppService = medidorAppService;

        [HttpGet("{instalacao}/{lote}")]
        public IActionResult Get(string? instalacao, int? lote)
        {
            Resposta<MedidorDTO> rMedidores = _medidorAppService.Consultar(instalacao, lote);

            if (rMedidores.IsSucesso)
            {
                return Ok(rMedidores.Resultado);
            }
            else
            {
                return BadRequest(rMedidores.Mensagem);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            Resposta<List<MedidorDTO>> rMedidores = _medidorAppService.Consultar();

            if (rMedidores.IsSucesso)
            {
                return Ok(rMedidores.Resultado);
            }
            else
            {
                return BadRequest(rMedidores.Mensagem);
            }
        }
    }
}

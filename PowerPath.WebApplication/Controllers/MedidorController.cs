using Microsoft.AspNetCore.Mvc;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.WebApplication.Controllers
{
    public class MedidorController(IMedidorApplicationService medidorAppService) : Controller
    {
        private readonly IMedidorApplicationService _medidorAppService = medidorAppService;

        public IActionResult Index()
        {
            Resposta<List<string>> rOperadoras = _medidorAppService.ListarOperadoras();
            Resposta<List<MedidorDTO>> rMedidores = _medidorAppService.Consultar(true);

            if (rOperadoras.IsSucesso)
            {
                ViewBag.Operadoras = rOperadoras.Resultado;
            } else
            {
                ViewBag.Operadoras = new List<string>();
            }

            if (rMedidores.IsSucesso)
            {
                return View(rMedidores.Resultado);
            } else
            {
                return View(new List<MedidorDTO>());
            }
        }

        [HttpDelete]
        public IActionResult Excluir(string? instalacao, int? lote)
        {
            Resposta<MedidorDTO> rExcluirMedidor = _medidorAppService.Excluir(instalacao, lote);

            if (rExcluirMedidor.IsSucesso)
            {
                return Ok(new {mensagem = "O medidor foi excluído com sucesso!" });
            }
            else
            {
                return BadRequest(new { mensagem = rExcluirMedidor.Mensagem });
            }
        }
    }
}

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
            }
            else
            {
                ViewBag.Operadoras = new List<string>();
            }

            if (rMedidores.IsSucesso)
            {
                return View(rMedidores.Resultado);
            }
            else
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
                return Ok(new { mensagem = "O medidor foi excluído com sucesso!" });
            }
            else
            {
                return BadRequest(new { mensagem = rExcluirMedidor.Mensagem });
            }
        }

        [HttpGet]
        public IActionResult Inserir()
        {
            MedidorDTO medidor = new()
            {
                IsAlteracao = false
            };

            Resposta<List<string>> rOperadoras = _medidorAppService.ListarOperadoras();

            if (rOperadoras.IsSucesso)
            {
                ViewBag.Operadoras = rOperadoras.Resultado;
            }
            else
            {
                ViewBag.Operadoras = new List<string>();
            }

            return View(medidor);
        }

        [HttpGet]
        public IActionResult Alterar(string? instalacao, int? lote)
        {
            Resposta<List<string>> rOperadoras = _medidorAppService.ListarOperadoras();

            if (rOperadoras.IsSucesso)
            {
                ViewBag.Operadoras = rOperadoras.Resultado;
            }
            else
            {
                ViewBag.Operadoras = new List<string>();
            }

            Resposta<MedidorDTO> rConsultarMedidor = _medidorAppService.Consultar(instalacao, lote);

            if (rConsultarMedidor.IsSucesso)
            {
                MedidorDTO medidor = rConsultarMedidor.Resultado!;
                medidor.IsAlteracao = true;
                return View(medidor);
            }
            else
            {
                TempData["MensagemErro"] = rConsultarMedidor.Mensagem;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Salvar(MedidorDTO medidor)
        {
            Resposta<List<string>> rOperadoras = _medidorAppService.ListarOperadoras();

            if (rOperadoras.IsSucesso)
            {
                ViewBag.Operadoras = rOperadoras.Resultado;
            }
            else
            {
                ViewBag.Operadoras = new List<string>();
            }

            if (medidor.IsAlteracao.HasValue && medidor.IsAlteracao.Value)
            {
                Resposta<MedidorDTO> rAlterarMedidor = _medidorAppService.Alterar(medidor.Instalacao, medidor.Lote, medidor.Operadora,
                    medidor.Fabricante, medidor.Modelo, medidor.Versao);

                if (rAlterarMedidor.IsSucesso)
                {
                    TempData["MensagemSucesso"] = "Medidor alterado com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MensagemErro"] = rAlterarMedidor.Mensagem;
                    return View("Alterar", medidor);
                }
            }
            else
            {
                Resposta<MedidorDTO> rInserirMedidor = _medidorAppService.Inserir(medidor.Instalacao, medidor.Lote, medidor.Operadora,
                    medidor.Fabricante, medidor.Modelo, medidor.Versao);

                if (rInserirMedidor.IsSucesso)
                {
                    TempData["MensagemSucesso"] = "Medidor inserido com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MensagemErro"] = rInserirMedidor.Mensagem;
                    return View("Inserir", medidor);
                }
            }
        }
    }
}

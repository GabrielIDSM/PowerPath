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

        [HttpDelete("{instalacao}/{lote}")]
        public IActionResult Delete(string? instalacao, int? lote)
        {
            Resposta<MedidorDTO> rMedidores = _medidorAppService.Excluir(instalacao, lote);

            if (rMedidores.IsSucesso)
            {
                return Ok("Medidor excluído com sucesso!");
            }
            else
            {
                return BadRequest(rMedidores.Mensagem);
            }
        }

        [HttpPost("Salvar")]
        public IActionResult Salvar([FromBody] MedidorDTO medidor)
        {
            if (medidor.IsAlteracao.HasValue && medidor.IsAlteracao.Value)
            {
                Resposta<MedidorDTO> rAlterarMedidor = _medidorAppService.Alterar(medidor.Instalacao, medidor.Lote, medidor.Operadora,
                    medidor.Fabricante, medidor.Modelo, medidor.Versao);

                if (rAlterarMedidor.IsSucesso)
                {
                    return Ok("Medidor alterado com sucesso!");
                }
                else
                {
                    return BadRequest(rAlterarMedidor.Mensagem);
                }
            }
            else
            {
                Resposta<MedidorDTO> rInserirMedidor = _medidorAppService.Inserir(medidor.Instalacao, medidor.Lote, medidor.Operadora,
                    medidor.Fabricante, medidor.Modelo, medidor.Versao);

                if (rInserirMedidor.IsSucesso)
                {
                    return Ok("Medidor inserido com sucesso!");
                }
                else
                {
                    return BadRequest(rInserirMedidor.Mensagem);
                }
            }
        }
    }
}

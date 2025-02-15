using Microsoft.AspNetCore.Mvc;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacaoController(IUsuarioApplicationService usuarioAppService) : ControllerBase
    {
        private readonly IUsuarioApplicationService _usuarioAppService = usuarioAppService;

        [HttpPost]
        public IActionResult ObterToken([FromBody] UsuarioDTO usuario)
        {
            Resposta<string> rToken = _usuarioAppService.Autenticar(usuario.Nome, usuario.Senha);

            if (rToken.IsSucesso)
            {
                return Ok(new { Token = rToken.Resultado });
            }
            else
            {
                return Unauthorized("Usuário e/ou senha incorretos.");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.WebApplication.Controllers
{
    public class LoginController(IUsuarioApplicationService usuarioAppService) : Controller
    {
        private readonly IUsuarioApplicationService _usuarioAppService = usuarioAppService;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Autenticar(UsuarioDTO usuario)
        {
            Resposta<string> rToken = _usuarioAppService.Autenticar(usuario.Nome, usuario.Senha);

            if (rToken.IsSucesso)
            {
                Response.Cookies.Append("AuthToken", rToken.Resultado!, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                Response.Cookies.Append("Username", usuario.Nome!, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                return RedirectToAction("Index", "Medidor");
            }
            else
            {
                TempData["MensagemErro"] = rToken.Mensagem;
                return View("Index", usuario);
            }
        }
    }
}

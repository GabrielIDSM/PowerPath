using Microsoft.AspNetCore.Mvc;

namespace PowerPath.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Medidores()
        {
            return View();
        }

        public IActionResult Log()
        {
            return View();
        }
    }
}

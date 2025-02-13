using Microsoft.AspNetCore.Mvc;

namespace PowerPath.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

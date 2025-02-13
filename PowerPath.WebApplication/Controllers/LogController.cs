using Microsoft.AspNetCore.Mvc;

namespace PowerPath.WebApplication.Controllers
{
    public class LogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

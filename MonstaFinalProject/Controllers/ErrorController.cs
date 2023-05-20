using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

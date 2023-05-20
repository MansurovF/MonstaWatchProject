using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

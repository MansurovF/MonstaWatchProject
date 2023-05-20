using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

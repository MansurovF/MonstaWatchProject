using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

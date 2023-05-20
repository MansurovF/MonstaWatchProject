using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

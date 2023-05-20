using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

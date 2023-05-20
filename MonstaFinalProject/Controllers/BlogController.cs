using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

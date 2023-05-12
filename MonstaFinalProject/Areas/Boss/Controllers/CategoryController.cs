using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Areas.Boss.Controllers
{
    public class CategoryController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Areas.Boss.Controllers
{
    [Area("boss")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

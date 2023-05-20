using Microsoft.AspNetCore.Mvc;

namespace MonstaFinalProject.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

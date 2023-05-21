using Microsoft.AspNetCore.Mvc;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Models;

namespace MonstaFinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

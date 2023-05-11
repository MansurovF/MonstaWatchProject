using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels;

namespace MonstaFinalProject.Areas.Boss.Controllers
{
    [Area("boss")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        public BrandController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Brand> brands = await _context.Brands.Include(b=>b.Products).Where(b => b.IsDeleted == false).ToListAsync();
            return View(brands);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }
            if (await _context.Brands.AnyAsync(b => b.IsDeleted == false && b.Name.ToLower() == brand.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", $"{brand.Name} adinda brand movcuddur");
                return View(brand);
            }
            brand.Name = brand.Name.Trim();
            brand.CreatedAt = DateTime.UtcNow.AddHours(4);
            brand.CreatedBy = "System";


            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
    }
}

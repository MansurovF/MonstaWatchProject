using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Brand> brands = await _context.Brands.Include(b=>b.Products).Where(b => b.IsDeleted == false).ToListAsync();
            return View(brands);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Detail(int? Id)
        {
            if (Id == null) return BadRequest();
            Brand brand = await _context.Brands
                .Include(b => b.Products.Where(p => p.IsDeleted == false))
                .FirstOrDefaultAsync(b => b.Id == Id && b.IsDeleted == false);
            if (brand == null) return NotFound();
            return View(brand);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
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


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id && b.IsDeleted == false);

            if (brand == null) return NotFound();
            return View(brand);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(int? Id,Brand brand)
        {
            if (!ModelState.IsValid) return View(brand);

            if (Id == null) return BadRequest();

            if (Id != brand.Id) return BadRequest();

            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == Id && b.IsDeleted == false);
            if (dbBrand == null) return NotFound();

            if (await _context.Brands.AnyAsync(b => b.IsDeleted == false && b.Name.ToLower() == brand.Name.Trim().ToLower() && b.Id != Id))
            {
                ModelState.AddModelError("Name", $"Bu Adda {brand.Name} Brand Artiq Movcuddur");
                return View(brand);
            }

            dbBrand.Name = brand.Name.Trim();
            dbBrand.UpdatedAt = DateTime.UtcNow.AddHours(4);
            dbBrand.UpdatedBy = "System";
            await _context.SaveChangesAsync();
            return RedirectToActionPermanent(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands
            .Include(b => b.Products.Where(p => p.IsDeleted == false ))
            .FirstOrDefaultAsync(b => b.Id == id && b.IsDeleted == false);
          
            if (brand == null) return NotFound();
          
            brand.IsDeleted = true;
            brand.DeletedBy = "System";
            brand.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            IEnumerable<Brand> brands = await _context.Brands.Include(b => b.Products).Where(b => b.IsDeleted == false).ToListAsync();
            

            return PartialView("_BrandIndexPartial", brands);
        }
    }
}

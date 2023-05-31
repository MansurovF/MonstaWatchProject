using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels;
using System.Data;
using System.Drawing.Drawing2D;

namespace MonstaFinalProject.Areas.Boss.Controllers
{
    [Area("boss")]
    public class ColorController :Controller
    {
        private readonly AppDbContext _context;
        public ColorController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Color>colors = await _context.Colors.Include(c =>c.Products).Where(c=>c.IsDeleted == false).ToListAsync();
            return View(colors);
        }

        public async Task<IActionResult> Detail(int? Id)
        {
            if (Id == null) return BadRequest();
            Color color= await _context.Colors
                .Include(c => c.Products.Where(p => p.IsDeleted == false))
                .FirstOrDefaultAsync(c => c.Id == Id && c.IsDeleted == false);
            if (color == null) return NotFound();

            return View(color);
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Create(Color color)
        {
            if (!ModelState.IsValid)
            {
                return View(color);
            }
            if (await _context.Colors.AnyAsync(c => c.IsDeleted == false && c.Name.ToLower() == color.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", $"{color.Name} adinda reng movcuddur");
                return View(color);
            }

            color.Name=color.Name.Trim();
            color.CreatedAt = DateTime.UtcNow.AddHours(4);
            color.CreatedBy = "System";

            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            if (color == null) return NotFound();
            return View(color);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? Id, Color color)
        {
            if (!ModelState.IsValid) return View(color);

            if (Id == null) return BadRequest();
            if(Id != color.Id)return BadRequest();

            Color dbcolor= await _context.Colors.FirstOrDefaultAsync(c => c.Id == Id && c.IsDeleted == false);
            if (dbcolor == null) return NotFound();

            if (await _context.Colors.AnyAsync(c => c.IsDeleted == false && c.Name.ToLower() == color.Name.Trim().ToLower() && c.Id != Id))
            {
                ModelState.AddModelError("Name", $"Bu Adda {color.Name} Color Artiq Movcuddur");
                return View(color);
            }

            dbcolor.Name= color.Name.Trim();
            dbcolor.UpdatedAt= DateTime.UtcNow.AddHours(4);
            dbcolor.UpdatedBy = "System";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Color color = await _context.Colors
            .Include(c => c.Products.Where(p => p.IsDeleted == false))
            .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            if (color == null) return NotFound();

            color.IsDeleted = true;
            color.DeletedBy = "System";
            color.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            IEnumerable<Color> colors = await _context.Colors.Include(c => c.Products).Where(c => c.IsDeleted == false).ToListAsync();


            return PartialView("_ColorIndexPartial", colors);
        }

    }
}

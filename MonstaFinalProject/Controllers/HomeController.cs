using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels.HomeViewModels;
using System.Collections.Generic;

namespace MonstaFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            

            HomeVM homeVM = new HomeVM
            {
                Sliders = await _context.Sliders.Where(s => s.IsDeleted == false).ToListAsync(),
                Featured = await _context.Products.Where(c => c.IsDeleted == false && c.IsFeatured).ToListAsync(),
                BestSelling = await _context.Products.Where(c => c.IsDeleted == false && c.IsBestSelling).ToListAsync(),
                Onsale = await _context.Products.Where(c => c.IsDeleted == false && c.IsOnsale).ToListAsync()
            };

            return View(homeVM);
        }
    }
}

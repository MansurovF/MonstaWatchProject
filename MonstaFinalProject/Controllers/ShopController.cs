using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels;
using MonstaFinalProject.ViewModels.HomeViewModels;
using MonstaFinalProject.ViewModels.ShopViewModels;

namespace MonstaFinalProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.Where(s => s.IsDeleted == false).Take(3).ToListAsync();
            ShopVM shopVM = new ShopVM
            {
                Products = products
            };

            IEnumerable<Category> categories = await _context.Categories.Where(ca => ca.IsDeleted == false && ca.IsMain)
               .Include(ca => ca.Children.Where(ct => ct.IsDeleted == false && ct.IsMain == false))
               .Include(ca => ca.Products).ToListAsync();

            IEnumerable<Color> authors = await _context.Colors.Where(c => c.IsDeleted == false).ToListAsync();

            Product product = _context.Products.OrderBy(p =>  p.Price).First();
            double minValue = (product.Price);

            product = _context.Products.OrderByDescending(p => p.Price).First();
            double maxValue = ( product.Price);

            return View(shopVM);

            ShopVM shopVM1 = new ShopVM
            {
                //Categories = categories,
                //Authors = authors,
                //AllProducts = newProducts/*.ToList()*/,
                //SortSelect = (sortSelect == -1 ? 0 : sortSelect),
                
            };
        }

        public async Task<IActionResult> Filter( int? sortby )
        {
            int sortSelect = -1;


            return View();
        }

        //public async Task<IActionResult> Range(double? min, double? max)
        //{
        //    List<Product> products = await _context.Products.Where(s => s.IsDeleted == false && s.Price >= min && s.Price <= max).ToListAsync();
        //    ShopVM shopVM = new ShopVM
        //    {
        //        Products = products
        //    };


        //    return PartialView("_ShopViewPartial", products);
        //}
    }
}

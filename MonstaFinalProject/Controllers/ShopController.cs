using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
           

            IEnumerable<Category> categories = await _context.Categories.Where(ca => ca.IsDeleted == false && ca.IsMain)
               .Include(ca => ca.Children.Where(ct => ct.IsDeleted == false && ct.IsMain == false))
               .Include(ca => ca.Products).ToListAsync();

            IEnumerable<Brand> brands = await _context.Brands.Where(b => b.IsDeleted == false )
               .Include(b => b.Products).ToListAsync();

            IEnumerable<Color> authors = await _context.Colors.Where(c => c.IsDeleted == false).ToListAsync();

            Product product = _context.Products.OrderBy(p =>  p.Price).First();
            double minValue = (product.Price);

            product = _context.Products.OrderByDescending(p => p.Price).First();
            double maxValue = ( product.Price);

            ShopVM shopVM = new ShopVM
            {
                Products = products,
                MinimumPrice = minValue,
                MaximumPrice = maxValue,
                Categories = categories,
                Brands = brands,
            };

            return View(shopVM);

            ShopVM shopVM1 = new ShopVM
            {
                //Categories = categories,
                //Authors = authors,
                //AllProducts = newProducts/*.ToList()*/,
                //SortSelect = (sortSelect == -1 ? 0 : sortSelect),
                
            };
        }

        public async Task<IActionResult> Filter(int? categoryId, int? brandId, double? min, double? max, int? val)
        {
            IEnumerable<Product> products = await _context.Products.Where(p => p.IsDeleted == false).ToListAsync();

            if (categoryId != null && categoryId >= 0)
            {
                if (await _context.Categories.AnyAsync(ca => ca.Id == categoryId && ca.IsDeleted == false))
                {
                    products = products
                     .Where(p => p.IsDeleted == false && p.CategoryId == categoryId).ToList();
                }
            }
            if (brandId != null && brandId >= 0)
            {
                if (await _context.Brands.AnyAsync(c => c.Id == brandId && c.IsDeleted == false))
                {
                    products = products
                   .Where(p => p.IsDeleted == false && p.BrandId == brandId).ToList();
                }
            }
            if (min != null && min >= 0)
            {
                if (max != null && max >= 0)
                {
                    if (min != max && min < max)
                    {
                        products = products
                             .Where(
                            p => p.IsDeleted == false &&
                        (p.Price > 0 ? p.Price >= min : p.Price >= min) &&
                        (p.Price > 0 ? p.Price <= max : p.Price <= max)
                        )
                      .ToList();
                    }
                }
            }

            return PartialView("_ShopViewPartial", products.Take(3));
        }

    }
}

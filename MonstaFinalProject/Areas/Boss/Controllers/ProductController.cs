using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Extenions;
using MonstaFinalProject.Helpers;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels;

namespace MonstaFinalProject.Areas.Boss.Controllers
{
    [Area("boss")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<Product> queries = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .Where(p => p.IsDeleted == false);

            return View(PageNatedList<Product>.Create(queries, pageIndex, 3, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(c => c.IsDeleted == false && c.Id == id);
                

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _context.Brands.Where(ca => ca.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(ca => ca.IsDeleted == false).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(ca => ca.IsDeleted == false).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //nese create elemir selectiste problem gosterir createcshtmlde
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Brands = await _context.Brands.Where(ca => ca.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(ca => ca.IsDeleted == false).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(ca => ca.IsDeleted == false).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(product);
            }
            if (product.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "Categoriya Mutleq secilmelidir!");
                return View(product);
            }
            if (product.BrandId == null)
            {
                ModelState.AddModelError("BrandId", "Brend mutleq secilmelidir!");
                return View(product);
            }
            if (product.ColorId == null)
            {
                ModelState.AddModelError("BrandId", "Color mutleq secilmelidir!");
                return View(product);
            }

            if (!await _context.Categories.AnyAsync(ca => ca.IsDeleted == false && ca.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Duzgun Categoriya secin!");
                return View(product);
            }
            if (!await _context.Brands.AnyAsync(ca => ca.IsDeleted == false && ca.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Duzgun brend secin!");
                return View(product);
            }
            if (!await _context.Colors.AnyAsync(ca => ca.IsDeleted == false && ca.Id == product.ColorId))
            {
                ModelState.AddModelError("ColorId", "Duzgun color secin!");
                return View(product);
            }

            if (product.MainFile != null)
            {
                if (product.MainFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainFile", $"{product.MainFile.FileName} adli shekil novu duzgun deyil");
                    return View(product);
                }
                if (product.MainFile.CheckFileLenght(300))
                {
                    ModelState.AddModelError("MainFile", $"{product.MainFile.FileName} adli shekil olcusu coxdur!");
                    return View(product);
                }

                product.MainImage = await product.MainFile.CreateFileAsync(_env, "assets", "images");
            }
            else
            {
                ModelState.AddModelError("MainFile", "MainFile mutleqdir!");
                return View(product);
            }

            if (product.HoverFile != null)
            {
                if (product.HoverFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("HoverFile", $"{product.HoverFile.FileName} adli shekil novu duzgun deyil");
                    return View(product);
                }
                if (product.HoverFile.CheckFileLenght(300))
                {
                    ModelState.AddModelError("HoverFile", $"{product.HoverFile.FileName} adli shekil olcusu coxdur!");
                    return View(product);
                }

                product.HoverImage = await product.HoverFile.CreateFileAsync(_env, "assets", "images");
            }
            else
            {
                ModelState.AddModelError("HoverFile", "HoverFile mutleqdir!");
                return View(product);
            }

            if (product.Files != null && product.Files.Count() > 6)
            {
                ModelState.AddModelError("Files", "File sayi coxdur! 6 dan cox fayl gondermek olmaz!");
                return View(product);
            }
            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();
                foreach (IFormFile file in product.Files)
                {
                    if (file.CheckFileContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("Files", $"{file.FileName} adli shekil novu duzgun deyil");
                        return View(product);
                    }
                    if (file.CheckFileLenght(300))
                    {
                        ModelState.AddModelError("Files", $"{file.FileName} adli shekil olcusu coxdur!");
                        return View(product);
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateFileAsync(_env, "assets", "images"),
                        CreatedAt = DateTime.UtcNow.AddHours(4),
                        CreatedBy = "System"
                    };

                    productImages.Add(productImage);


                }
                product.ProductImages = productImages;
            }
            else
            {
                ModelState.AddModelError("Files", "Sekil mutleq secilmelidir!");
                return View(product);
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsDeleted == false))
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (product == null) return NotFound();

            ViewBag.Brands = await _context.Brands.Where(ca => ca.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(ca => ca.IsDeleted == false).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(ca => ca.IsDeleted == false).ToListAsync();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int? id, int? imageId)
        {
            if (id == null) return BadRequest();

            if (imageId == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsDeleted == false))
                .FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);

            if (product == null) return NotFound();

            if (!product.ProductImages.Any(pi => pi.Id == imageId)) return BadRequest();

            if (product.ProductImages.Count() <= 1)
            {
                return BadRequest();
            }

            product.ProductImages.FirstOrDefault(p => p.Id == imageId).IsDeleted = true;
            product.ProductImages.FirstOrDefault(p => p.Id == imageId).DeletedBy = "System";
            product.ProductImages.FirstOrDefault(p => p.Id == imageId).DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            FileHelper.DeleteFile(product.ProductImages.FirstOrDefault(p => p.Id == imageId).Image, _env, "assets", "images");

            return PartialView("_ProductImagePartial", product.ProductImages.Where(pi => pi.IsDeleted == false).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Product product)
        {
            ViewBag.Brands = await _context.Brands.Where(ca => ca.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(ca => ca.IsDeleted == false).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(ca => ca.IsDeleted == false).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (id == null)
            {
                return BadRequest();
            }

            if (id != product.Id) return BadRequest();

            Product dbproduct = await _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsDeleted == false))
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (dbproduct == null) return NotFound();

            if (product.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "Categoriya Mutleq secilmelidir!");
                return View(product);
            }
            if (product.BrandId == null)
            {
                ModelState.AddModelError("BrandId", "Brend mutleq secilmelidir!");
                return View(product);
            }
            if (product.ColorId == null)
            {
                ModelState.AddModelError("BrandId", "Color mutleq secilmelidir!");
                return View(product);
            }

            if (!await _context.Categories.AnyAsync(ca => ca.IsDeleted == false && ca.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Duzgun Categoriya secin!");
                return View(product);
            }
            if (!await _context.Brands.AnyAsync(ca => ca.IsDeleted == false && ca.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Duzgun brend secin!");
                return View(product);
            }
            if (!await _context.Colors.AnyAsync(ca => ca.IsDeleted == false && ca.Id == product.ColorId))
            {
                ModelState.AddModelError("ColorId", "Duzgun color secin!");
                return View(product);
            }

            if (product.MainFile != null)
            {
                if (product.MainFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainFile", $"{product.MainFile.FileName} adli shekil novu duzgun deyil");
                    return View(product);
                }
                if (product.MainFile.CheckFileLenght(300))
                {
                    ModelState.AddModelError("MainFile", $"{product.MainFile.FileName} adli shekil olcusu coxdur!");
                    return View(product);
                }
                FileHelper.DeleteFile(dbproduct.MainImage, _env, "assets");

                dbproduct.MainImage = await product.MainFile.CreateFileAsync(_env, "assets", "images");
            }

            if (product.HoverFile != null)
            {
                if (product.HoverFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("HoverFile", $"{product.HoverFile.FileName} adli shekil novu duzgun deyil");
                    return View(product);
                }
                if (product.HoverFile.CheckFileLenght(300))
                {
                    ModelState.AddModelError("HoverFile", $"{product.HoverFile.FileName} adli shekil olcusu coxdur!");
                    return View(product);
                }

                FileHelper.DeleteFile(dbproduct.HoverImage, _env, "assets", "images");

                dbproduct.HoverImage = await product.HoverFile.CreateFileAsync(_env, "assets", "images");
            }

            int canUpload = 6 - (dbproduct.ProductImages != null ? dbproduct.ProductImages.Count() : 0);

            if (product.Files != null && canUpload < product.Files.Count())
            {
                ModelState.AddModelError("Files", $"Maksimum {canUpload} shekil yukleye bilersiz!");
                return View(dbproduct);
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();
                foreach (IFormFile file in product.Files)
                {
                    if (file.CheckFileContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("Files", $"{file.FileName} adli shekil novu duzgun deyil");
                        return View(product);
                    }
                    if (file.CheckFileLenght(300))
                    {
                        ModelState.AddModelError("Files", $"{file.FileName} adli shekil olcusu coxdur!");
                        return View(product);
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateFileAsync(_env, "assets", "images"),
                        CreatedAt = DateTime.UtcNow.AddHours(4),
                        CreatedBy = "System"
                    };

                    productImages.Add(productImage);


                }
                dbproduct.ProductImages.AddRange(productImages);
            }

            dbproduct.Title= product.Title;
            dbproduct.Description= product.Description;
            dbproduct.Price= product.Price;
            dbproduct.Count = product.Count;
            dbproduct.IsBestSelling= product.IsBestSelling;
            dbproduct.IsFeatured= product.IsFeatured;
            dbproduct.IsOnsale= product.IsOnsale;
            dbproduct.UpdatedBy = "System";
            dbproduct.UpdatedAt= DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Product product = await _context.Products
             .Include(p => p.ProductImages.Where(pi => pi.IsDeleted == false))
             .Include(p => p.Color)
             .Include(p => p.Brand)
             .Include(p => p.Category)
             .FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);
            if (product == null) return NotFound();
            ViewBag.Brands = await _context.Brands.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(b => b.IsDeleted == false).ToListAsync();
            return View(product);
        }

    }
}

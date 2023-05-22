using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Helpers;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels;
using System.Security.Claims;

namespace MonstaFinalProject.Areas.Boss.Controllers
{
    [Area("boss")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            IQueryable<Category> query = _context.Categories
                .Include(ca => ca.Products.Where(p => p.IsDeleted == false))
                .Where(ca => ca.IsDeleted == false && ca.IsMain)
                .OrderByDescending(ca => ca.Id);

            return View(PageNatedList<Category>.Create(query, pageIndex, 3, 8));
        }
        //public async Task<IActionResult> Index(int pageIndex = 1)
        //{
        //    //IEnumerable<Category> categories = await _context.Categories
        //    //    .Include(ca => ca.Products.Where(p => p.IsDeleted == false))
        //    //    .Where(ca => ca.IsDeleted == false && ca.IsMain)
        //    //    .OrderByDescending(ca => ca.Id)
        //    //    .ToListAsync();

        //    //int totalCount = categories.Count();
        //    //categories = categories.Skip((pageIndex -1)*3).Take(3).ToList();

        //    return View();


        //}

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories
                .Include(ca => ca.Children.Where(ch => ch.IsDeleted == false))
                .ThenInclude(cb => cb.Products.Where(p => p.IsDeleted == false))
                .Include(ca => ca.Products.Where(p => p.IsDeleted == false))
                .FirstOrDefaultAsync(ca => ca.IsDeleted == false && ca.Id == id);
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            ViewBag.MainCategories = await _context.Categories.Where(ca => ca.IsDeleted == false && ca.IsMain).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await _context.Categories.AnyAsync(ca => ca.IsDeleted == false && ca.Name.ToLower() == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", $"{category.Name} add categoryartiq movcuddur!");
                return View(category);
            }

            if (category.IsMain)
            {
                if (category.File?.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("File", "Uygun Type Deyil, Yalniz JPEG/JPG type ola biler!");
                    return View();
                }
                if ((category.File?.Length / 1024) > 300)
                {
                    ModelState.AddModelError("File", "File-in olcusu 300Kb-i kece bilmez");
                    return View();
                }

                ////int lastIndex = category.File.FileName.LastIndexOf(".");
                ////string name = category.File.FileName.Substring(lastIndex);

                ////string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}_{Guid.NewGuid()}{name}";

                ////string fullPath = Path.Combine(_env.WebRootPath, "assets", "images", fileName);


                //using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                //{
                //    await category.File.CopyToAsync(stream);
                //}

                category.ParentId = null;
            }
            else
            {
                if (category.ParentId == null)
                {
                    ModelState.AddModelError("ParentId", "Parent mutleq secilmelidir!");
                    return View(category);
                }
                if (!await _context.Categories.AnyAsync(ca => ca.IsDeleted == false && ca.Id == category.ParentId && ca.IsMain))
                {
                    ModelState.AddModelError("ParentId", "Parent duzgun secilmelidir!");
                    return View(category);
                }
                
            }

            category.Name = category.Name.Trim();
            category.CreatedAt = DateTime.UtcNow.AddHours(4);
            category.CreatedBy = "System";

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? Id)
        {
            if (Id == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(ca => ca.Id == Id && ca.IsDeleted == false);

            if (category == null) return NotFound();

            ViewBag.MainCategories = await _context.Categories.Where(ca => ca.IsDeleted == false && ca.IsMain).ToListAsync();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            ViewBag.MainCategories = await _context.Categories.Where(ca => ca.IsDeleted == false && ca.IsMain).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (id == null) return BadRequest();
            if (id !=category.Id) return BadRequest();

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(ca => ca.Id == id && ca.IsDeleted == false);

            if (category == null) return NotFound();

            if (await _context.Categories.AnyAsync(ca => ca.IsDeleted == false && ca.Name.ToLower() == category.Name.Trim().ToLower() && ca.Id != category.Id))
            {
                ModelState.AddModelError("Name", $"{category.Name} add categoryartiq movcuddur!");
                return View(category);
            }

            //if (dbCategory.IsMain != category.IsMain)
            //{
            //    ModelState.AddModelError("IsMain", "Category veziyyeti deyise bilmez");
            //    return View(dbCategory);
            //}

            if (dbCategory.IsMain && category.File != null)
            {
                if (category.File?.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("File", "Uygun Type Deyil, Yalniz JPEG/JPG type ola biler!");
                    return View();
                }
                if ((category.File?.Length / 1024) > 300)
                {
                    ModelState.AddModelError("File", "File-in olcusu 300Kb-i kece bilmez");
                    return View();
                }

                //FileHelper.DeleteFile(dbCategory.Image,_env,"assets","images")

                int lastIndex = category.File.FileName.LastIndexOf(".");
                string name = category.File.FileName.Substring(lastIndex);

                string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}_{Guid.NewGuid()}{name}";

                string fullPath = Path.Combine(_env.WebRootPath, "assets", "images", fileName);


                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    await category.File.CopyToAsync(stream);
                }

                //dbCategory.ParentId = null;
            }


            else
            {
                if (category.ParentId != dbCategory.ParentId)
                {
                    if (category.ParentId == null)
                    {
                        ModelState.AddModelError("ParentId", "Parent mutleq secilmelidir!");
                        return View(category);
                    }
                    if (!await _context.Categories.AnyAsync(ca => ca.IsDeleted == false && ca.Id == category.ParentId && ca.IsMain))
                    {
                        ModelState.AddModelError("ParentId", "Parent duzgun secilmelidir!");
                        return View(category);
                    }

                    dbCategory.ParentId = category.ParentId;
                }
            }

            dbCategory.Name = category.Name.Trim();
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);
            dbCategory.UpdatedBy = "System";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //problem with delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id,int pageIndex=1)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories
               .Include(ca => ca.Children.Where(ch => ch.IsDeleted == false))
               .ThenInclude(ch => ch.Products.Where(p => p.IsDeleted == false))
               .Include(ca => ca.Products.Where(p => p.IsDeleted == false))
               .FirstOrDefaultAsync(ca => ca.IsDeleted == false && ca.Id == id);

            if (category == null) return NotFound();

            if (category.Children != null && category.Children.Count() > 0)
            {
                foreach (Category child in category.Children)
                {
                    child.IsDeleted = true;
                    child.DeletedBy = "System";
                    child.DeletedAt = DateTime.UtcNow.AddHours(4);

                    if (child.Products != null && child.Products.Count() > 0)
                    {
                        foreach (Product product in child.Products)
                        {
                            product.CategoryId = null;
                        }
                    }
                }
            }

            if (category.Products != null && category.Products.Count() > 0)
            {
                foreach (Product product in category.Products)
                {
                    product.CategoryId = null;
                }
            }

            category.IsDeleted = true;
            category.DeletedBy = "System";
            category.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IEnumerable<Category> categories=  await _context.Categories
                .Include(ca => ca.Products.Where(p => p.IsDeleted == false))
                .Where(ca => ca.IsDeleted == false && ca.IsMain)
                .OrderByDescending(ca => ca.Id).ToListAsync();

            return PartialView("_CategoryIndexPartial",PageNatedList<Category>.Create(categories.AsQueryable(),pageIndex , 3,8));
        }

        //[HttpGet]

        //public async Task<IActionResult> DeleteCategory(int? id)
        //{
        //    if (id == null) return BadRequest();

        //    Category category = await _context.Categories
        //       .Include(ca => ca.Children.Where(ch => ch.IsDeleted == false))
        //       .ThenInclude(ch => ch.Products.Where(p => p.IsDeleted == false))
        //       .Include(ca => ca.Products.Where(p => p.IsDeleted == false))
        //       .FirstOrDefaultAsync(ca => ca.IsDeleted == false && ca.Id == id);

        //    if (category == null) return NotFound();

        //    if (category.Children != null && category.Children.Count() > 0)
        //    {
        //        foreach (Category child in category.Children)
        //        {
        //            child.IsDeleted = true;
        //            child.DeletedBy = "System";
        //            child.DeletedAt = DateTime.UtcNow.AddHours(4);

        //            if (child.Products != null && child.Products.Count() > 0)
        //            {
        //                foreach (Product product in child.Products)
        //                {
        //                    product.CategoryId = null;
        //                }
        //            }
        //        }
        //    }

        //    if (category.Products != null && category.Products.Count() > 0)
        //    {
        //        foreach (Product product in category.Products)
        //        {
        //            product.CategoryId = null;
        //        }
        //    }

        //    category.IsDeleted = true;
        //    category.DeletedBy = "System";
        //    category.DeletedAt = DateTime.UtcNow.AddHours(4);

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}
    }
}

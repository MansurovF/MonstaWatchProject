using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels.BasketViewModels;
using MonstaFinalProject.ViewModels.WishlistViewModels;
using Newtonsoft.Json;

namespace MonstaFinalProject.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WishlistController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            string cookie = HttpContext.Request.Cookies["wishlist"];
            List<WishlistVM> wishlistVMs = new();

            if (!string.IsNullOrWhiteSpace(cookie))
            {
                wishlistVMs = JsonConvert.DeserializeObject<List<WishlistVM>>(cookie);

                foreach (WishlistVM wishlistVM in wishlistVMs)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == wishlistVM.Id);

                    if (product != null)
                    {
                        wishlistVM.Title = product.Title;
                        wishlistVM.Price = product.Price;
                        wishlistVM.Image = product.MainImage;
                    }
                }
            }
            ViewBag.Wishlist = cookie;
            return View(wishlistVMs);
        }

        public async Task<IActionResult> WishlistDetails()
        {
            string cookie = HttpContext.Request.Cookies["wishlist"];

            List<WishlistVM> wishlistVMs = JsonConvert.DeserializeObject<List<WishlistVM>>(cookie);

            foreach (WishlistVM wishlistVM in wishlistVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == wishlistVM.Id);

                if (product != null)
                {
                    wishlistVM.Title = product.Title;
                    wishlistVM.Price = product.Price;
                    wishlistVM.Image = product.MainImage;
                }
            }
            ViewBag.Wishlist = cookie;
            return PartialView("_WishlistIndexPartial", wishlistVMs);
        }

        public async Task<IActionResult> AddWishlist(int? id)
        {
            if (id == null) return BadRequest();
            if (!await _context.Products.AnyAsync(p => p.IsDeleted == false && p.Id == id)) return NotFound();
            List<WishlistVM> wishlistVMs = new List<WishlistVM>();
            string cookie = HttpContext.Request.Cookies["wishlist"];
            if (string.IsNullOrWhiteSpace(cookie))
            {
                wishlistVMs = new List<WishlistVM>
                {
                    new WishlistVM{Id=(int)id}
                };
            }
            else
            {
                wishlistVMs = JsonConvert.DeserializeObject<List<WishlistVM>>(cookie);
                if (!wishlistVMs.Exists(b => b.Id == id))
                {
                    wishlistVMs.Add(new WishlistVM { Id = (int)id });
                }
            }

            cookie = JsonConvert.SerializeObject(wishlistVMs);
            HttpContext.Response.Cookies.Append("wishlist", cookie);
            foreach (WishlistVM wishlistVM in wishlistVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == wishlistVM.Id);
                if (product != null)
                {
                    wishlistVM.Title = product.Title;
                    wishlistVM.Price = product.Price;
                    wishlistVM.Image = product.MainImage;
                }
            }
            ViewBag.Wishlist = cookie;
            return PartialView("_WishlistIndexPartial", wishlistVMs);
        }
        //public async Task<IActionResult> DeleteWishlist(int? id)
        //{
        //    if (id == null) return BadRequest();
        //    if (!await _context.Products.AnyAsync(p => p.IsDeleted == false && p.Id == id)) return NotFound();
        //    string cookie = HttpContext.Request.Cookies["wishlist"];
        //    List<WishlistVM> wishlistVMs = new List<WishlistVM>();
        //    if (!string.IsNullOrWhiteSpace(cookie))
        //    {
        //        wishlistVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
        //        if (wishlistVMs.Exists(p => p.Id == id))
        //        {
        //            WishlistVM wishlistVM = wishlistVMs.Find(p => p.Id == id);
        //            if (wishlistVM.Count >= 1)
        //            {
        //                wishlistVMs.Remove(dbBasket);
        //            }
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    cookie = JsonConvert.SerializeObject(wishlistVMs);
        //    HttpContext.Response.Cookies.Append("wishlist", cookie);
        //    foreach (WishlistVM wishlistVM in wishlistVMs)
        //    {
        //        Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == wishlistVM.Id);
        //        if (product != null)
        //        {
        //            wishlistVM.Title = product.Title;
        //            wishlistVM.Price = product.Price;
        //            wishlistVM.Image = product.MainImage;
        //        }
        //    }
        //    return PartialView("_BasketIndexPartial", wishlistVMs);
        //}

        public async Task<IActionResult> RemoveWishlist(int? id)
        {
            if (id == null) return BadRequest();
            if (!await _context.Products.AnyAsync(p => p.IsDeleted == false && p.Id == id)) return NotFound();
            string cookie = HttpContext.Request.Cookies["wishlist"];
            List<WishlistVM> wishlistVMs = new List<WishlistVM>();
            if (!string.IsNullOrWhiteSpace(cookie))
            {
                wishlistVMs = JsonConvert.DeserializeObject<List<WishlistVM>>(cookie);
                if (wishlistVMs.Exists(p => p.Id == id))
                {
                    WishlistVM dbWishlist = wishlistVMs.Find(p => p.Id == id);
                    wishlistVMs.Remove(dbWishlist);
                }
                else
                {
                    
                    return BadRequest();
                }
            }
            cookie = JsonConvert.SerializeObject(wishlistVMs);
            HttpContext.Response.Cookies.Append("wishlist", cookie);
            foreach (WishlistVM wishlistVM in wishlistVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == wishlistVM.Id);
                if (product != null)
                {
                    wishlistVM.Title = product.Title;
                    wishlistVM.Price = product.Price;
                    wishlistVM.Image = product.MainImage;
                }
            }
            ViewBag.Wishlist = cookie;
            return PartialView("_WishlistIndexPartial", wishlistVMs);
        }
    }
}

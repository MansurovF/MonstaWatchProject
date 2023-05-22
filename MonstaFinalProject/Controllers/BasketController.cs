using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels.BasketViewModels;
using Newtonsoft.Json;

namespace MonstaFinalProject.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string cookie = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrEmpty(cookie))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);

                foreach (BasketVM basketVM in basketVMs)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == basketVM.Id);

                    if (product != null)
                    {
                        basketVM.Title = product.Title;
                        basketVM.Price = product.Price;
                        basketVM.Image = product.MainImage;
                        basketVM.Shipping = product.Shipping;
                        
                    }
                }
            }
            return View(basketVMs);
        }
        public async Task<IActionResult> AddBasket(int? Id)
        {
            if (Id == null) return BadRequest();

            if (!await _context.Products.AnyAsync(p => p.IsDeleted == false && p.Id == Id)) return NotFound();

            string cookie = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (string.IsNullOrWhiteSpace(cookie))
            {
                basketVMs = new List<BasketVM>
                {
                    new BasketVM {Id = (int)Id, Count= 1}
                };
            }
            else
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
                if (basketVMs.Exists(p => p.Id == Id))
                {
                    basketVMs.Find(b => b.Id == Id).Count += 1;
                }
                else
                {
                    basketVMs.Add(new BasketVM { Id = (int)Id, Count = 1 });
                };
            }

            cookie = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookie);

            foreach (BasketVM basketVM in basketVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == basketVM.Id);
                if (product != null)
                {
                    basketVM.Title = product.Title;
                    basketVM.Price = product.Price;
                    basketVM.Image = product.MainImage;
                }

            }

            return PartialView("_BasketIndexPartial", basketVMs);
        }

        public async Task<IActionResult> GetBasket()
        {
            string cookie = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = new List<BasketVM>();

            if (!string.IsNullOrWhiteSpace(cookie))
            { 
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
            }

            return PartialView("_BasketCartPartial", basketVMs);
        }

        public async Task<IActionResult> RemoveBasket(int? id)
        {
            

            if (id == null) return BadRequest();
            if (!await _context.Products.AnyAsync(p => p.IsDeleted == false && p.Id == id)) return NotFound();
            string cookie = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = new List<BasketVM>();
            if (!string.IsNullOrWhiteSpace(cookie))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
                if (basketVMs.Exists(p => p.Id == id))
                {
                    BasketVM dbBasket = basketVMs.Find(p => p.Id == id);

                    if (dbBasket.Count == 1)
                    {
                        basketVMs.Remove(dbBasket);

                    }
                    else
                    {
                        dbBasket.Count -= 1;
                    }

                }
                else
                {
                    return BadRequest();
                }
            }

            foreach (BasketVM basketVM in basketVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == basketVM.Id);

                if (product != null)
                {
                    basketVM.Title = product.Title;
                    basketVM.Price = product.Price;
                    basketVM.Image = product.MainImage; 
                }
            }
            cookie = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookie);

            return PartialView("_BasketIndexPartial", basketVMs);
        }

        public async Task<IActionResult> DeleteBasket(int? id)
        {
            if (id == null) return BadRequest();
            if (!await _context.Products.AnyAsync(p => p.IsDeleted == false && p.Id == id)) return NotFound();
            string cookie = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = new List<BasketVM>();
            if (!string.IsNullOrWhiteSpace(cookie))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
                if (basketVMs.Exists(p => p.Id == id))
                {
                    BasketVM dbBasket = basketVMs.Find(p => p.Id == id);
                    if (dbBasket.Count >= 1)
                    {
                        basketVMs.Remove(dbBasket);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            cookie = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookie);
            foreach (BasketVM basketVM in basketVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == basketVM.Id);
                if (product != null)
                {
                    basketVM.Title = product.Title;
                    basketVM.Price =  product.Price;
                    basketVM.Shipping = product.Shipping;
                    basketVM.Image = product.MainImage;
                }
            }
            return PartialView("_BasketIndexPartial", basketVMs);
        }


        public async Task<IActionResult> BasketDetails()
        {
            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            foreach (BasketVM basketVM in basketVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == basketVM.Id);

                if (product != null)
                {
                    basketVM.Title = product.Title;
                    basketVM.Price = product.Price;
                    basketVM.Image = product.MainImage;
                    basketVM.Shipping = product.Shipping;
                }
            }

            return PartialView("_BasketIndexPartial", basketVMs);
        }

        public async Task<IActionResult> MainBasket()
        {
            string cookie = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrEmpty(cookie))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);

                foreach (BasketVM basketVM in basketVMs)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == basketVM.Id);

                    if (product != null)
                    {
                        basketVM.Title = product.Title;
                        basketVM.Price = product.Price;
                        basketVM.Image = product.MainImage;
                        basketVM.Shipping = product.Shipping;
                    }
                }
            }
            return PartialView("_BasketIndexPartial", basketVMs);
        }

    }
}

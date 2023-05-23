using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Interfaces;
using MonstaFinalProject.Models;
using Newtonsoft.Json;
using MonstaFinalProject.ViewModels.BasketViewModels;
using MonstaFinalProject.ViewModels.WishlistViewModels;
using Newtonsoft.Json;

namespace MonstaFinalProject.Services
{
    public class LayoutService: ILayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpcontextAccessor;

        public LayoutService(AppDbContext context, IHttpContextAccessor httpcontextAccessor)
        {
            _context = context;
            _httpcontextAccessor = httpcontextAccessor;
        }
        public async Task<IDictionary<string, string>> GetSettings()
        {
            IDictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

            return settings;
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories
                .Include(ca => ca.Parent)
                .Where(ca => ca.IsDeleted == false && ca.IsMain).ToListAsync();
        }

        public async Task<List<BasketVM>> GetBaskets()
        {
          // List<Basket> baskets = null;
            List<BasketVM> basketVMs
                = new List<BasketVM>();
            string cookie =  _httpcontextAccessor.HttpContext.Request.Cookies["basket"];

            if (!string.IsNullOrWhiteSpace(cookie))
            {
                 basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);

                if (basketVMs != null && basketVMs.Count > 0)
                {
                    foreach (BasketVM basketVM in basketVMs)
                    {
                        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.Id);
                        if (product != null)
                        {
                            basketVM.Title = product.Title;
                            basketVM.Price = product.Price;
                            basketVM.Image = product.MainImage;
                            basketVM.Shipping = product.Shipping;
                        }
                    }
                    return basketVMs;
                }

                //else
                //{
                //    basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
                //    foreach (BasketVM basketVM1 in basketVMs)
                //    {
                //        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM1.Id);
                //        if (product != null)
                //        {
                //            basketVM1.Title = product.Title;
                //            basketVM1.Price = product.Price;
                //            basketVM1.Image = product.MainImage;
                //            basketVM1.Shipping = product.Shipping;
                //        }

                //    }
                //}
            }
            return new List<BasketVM>();
        }
    }
}

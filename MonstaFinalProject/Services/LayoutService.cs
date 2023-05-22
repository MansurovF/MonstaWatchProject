using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Interfaces;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels.BasketViewModels;

namespace MonstaFinalProject.Services
{
    public class LayoutService: ILayoutService
    {
        private readonly AppDbContext _context;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public LayoutService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            //_httpContextAccessor = httpContextAccessor;
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

        public Task<List<BasketVM>> GetBaskets()
        {
            throw new NotImplementedException();
        }

        //public Task<List<BasketVM>> GetBaskets()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Interfaces;
using MonstaFinalProject.Models;

namespace MonstaFinalProject.Services
{
    public class LayoutService: ILayoutService
    {
        private readonly AppDbContext _context;
        public LayoutService(AppDbContext context)
        {
            _context = context;
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
    }
}

using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels.BasketViewModels;

namespace MonstaFinalProject.Interfaces
{
    public interface ILayoutService
    {
        Task<IDictionary<string, string>> GetSettings();
        Task<IEnumerable<Category>> GetCategories();
        Task<List<BasketVM>> GetBaskets();
    }
}

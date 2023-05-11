using MonstaFinalProject.Models;

namespace MonstaFinalProject.Interfaces
{
    public interface ILayoutService
    {
        Task<IDictionary<string, string>> GetSettings();
    }
}

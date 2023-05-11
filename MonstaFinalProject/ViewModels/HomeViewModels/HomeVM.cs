using MonstaFinalProject.Models;

namespace MonstaFinalProject.ViewModels.HomeViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Product> Featured { get; set; }
        public IEnumerable<Product> BestSelling { get; set; }
        public IEnumerable<Product> Onsale { get; set; }

    }
}

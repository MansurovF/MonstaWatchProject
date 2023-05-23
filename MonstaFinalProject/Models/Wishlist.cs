namespace MonstaFinalProject.Models
{
    public class Wishlist:BaseEntity
    {
        
        public string? Title { get; set; }
        public string? Image { get; set; }
        public double? Price { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}

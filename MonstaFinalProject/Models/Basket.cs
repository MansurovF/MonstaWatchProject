namespace MonstaFinalProject.Models
{
    public class Basket:BaseEntity
    {
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int Count { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public double? Price { get; set; }
        public double? Shipping { get; set; }

    }
}

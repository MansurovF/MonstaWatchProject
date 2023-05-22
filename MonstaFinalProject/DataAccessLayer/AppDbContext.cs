using Microsoft.EntityFrameworkCore;
using MonstaFinalProject.Models;

namespace MonstaFinalProject.DataAccessLayer
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider>Sliders { get; set; }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Brand>Brands{ get; set; }
        public DbSet<Product>Products{ get; set; }
        public DbSet<Color>Colors{ get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Basket> Baskets { get; set; }
    }
}

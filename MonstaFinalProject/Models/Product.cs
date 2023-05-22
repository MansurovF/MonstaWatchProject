using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.ContentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MonstaFinalProject.Models
{
    public class Product:BaseEntity
    {
        [StringLength(255)]
        public string Title { get; set; }

        [Column(TypeName = "money")]
        public double Price { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [StringLength(255)]
        public string? MainImage { get; set; }
        [StringLength(255)]
        public string? HoverImage { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsBestSelling { get; set; }
        public bool IsOnsale { get; set; }
        
        public int Count { get; set; }


        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }
        public int? ColorId { get; set; }
        public Color? Color { get; set; }
        //public IEnumerable<Basket>? Baskets { get; set; }

        public List<ProductImage>? ProductImages { get; set; }

        [NotMapped]
        public IFormFile? MainFile { get; set; }
        [NotMapped]
        public IFormFile? HoverFile { get; set; }
        [NotMapped]
        public IEnumerable<IFormFile>? Files { get; set; }
    }
}

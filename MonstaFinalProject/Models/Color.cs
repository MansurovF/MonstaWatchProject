using System.ComponentModel.DataAnnotations;

namespace MonstaFinalProject.Models
{
    public class Color:BaseEntity
    {
        [StringLength(255)]
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }

    }
}

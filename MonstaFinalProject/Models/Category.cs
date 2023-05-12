using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MonstaFinalProject.Models
{
    public class Category:BaseEntity
    {
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsMain { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Category? Parent { get; set; }
        [NotMapped]
        public IEnumerable<Category>?  Children { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}

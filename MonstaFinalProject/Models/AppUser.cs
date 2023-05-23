using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MonstaFinalProject.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? SurName { get; set; }

        public bool IsActive { get; set; }
        //public List<Basket>? Baskets { get; set; }
        //public List<Wishlist>? Wishlists { get; set; }
    }
}

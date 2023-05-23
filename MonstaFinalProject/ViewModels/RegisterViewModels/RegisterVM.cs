using System.ComponentModel.DataAnnotations;

namespace MonstaFinalProject.ViewModels.RegisterViewModels
{
    public class ProfileVM
    {
        [StringLength(100)]
        public string? Name { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }
    }
}

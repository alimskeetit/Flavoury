using System.ComponentModel.DataAnnotations;

namespace Flavoury.ViewModels.Account
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

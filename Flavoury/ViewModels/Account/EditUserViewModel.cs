using System.ComponentModel.DataAnnotations;

namespace Flavoury.ViewModels.Account
{
    public class EditUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}

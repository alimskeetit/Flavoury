using System.ComponentModel.DataAnnotations;

namespace Flavoury.ViewModels.Account
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

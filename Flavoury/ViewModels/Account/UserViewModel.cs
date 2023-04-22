using System.ComponentModel.DataAnnotations;

namespace Flavoury.ViewModels.Account
{
    public class UserViewModel
    {
        [Required]
        public string UserName { get; set; }

        public IList<string> Role { get; set; }
    }
}

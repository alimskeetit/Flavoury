using System.ComponentModel.DataAnnotations;
using Flavoury.ViewModels.Ingredient;
using Flavoury.ViewModels.Tag;

namespace Flavoury.ViewModels.Recipe
{
    public class RecipeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        public string Description { get; set; } = null!;

        public ICollection<IngredientViewModel> Ingredients { get; set; } = new List<IngredientViewModel>();
        public ICollection<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

        public string Creator { get; set; } = null!;
    }
}

using Flavoury.ViewModels.Ingredient;
using Flavoury.ViewModels.Tag;
using System.ComponentModel.DataAnnotations;

namespace Flavoury.ViewModels.Recipe
{
    public class CreateRecipeViewModel
    {
        [Required(ErrorMessage = "Поле является обязательным")]
        public string Description { get; set; } = null!;

        public ICollection<CreateIngredientViewModel> Ingredients { get; set; } = new List<CreateIngredientViewModel>();
        public ICollection<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
    }
}

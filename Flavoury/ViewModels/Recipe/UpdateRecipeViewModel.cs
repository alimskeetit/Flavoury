using Flavoury.ViewModels.Ingredient;
using Flavoury.ViewModels.Tag;
using System.ComponentModel.DataAnnotations;

namespace Flavoury.ViewModels.Recipe
{
    public class UpdateRecipeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле является обязательным")]
        public string Description { get; set; } = null!;

        public ICollection<UpdateIngredientViewModel> Ingredients { get; set; } = new List<UpdateIngredientViewModel>();
        public ICollection<UpdateTagViewModel> Tags { get; set; } = new List<UpdateTagViewModel>();

    }
}

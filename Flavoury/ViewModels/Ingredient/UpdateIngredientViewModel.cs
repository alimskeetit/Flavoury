
using System.ComponentModel.DataAnnotations;

namespace Flavoury.ViewModels.Ingredient
{
    public class UpdateIngredientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Поле является обязательным")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть больше нуля")]
        public int Calories { get; set; }
    }
}

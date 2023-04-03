using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int Calories { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int RecipeId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Recipe? Recipe { get; set; }

    }
}

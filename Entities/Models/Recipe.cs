namespace Entities.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;
        
        public int Rating { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}

using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Flavoury.Services
{
    public class RecipeService: EntityService<Recipe>
    {
        public RecipeService(AppDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Recipe>> GetAllAsync() 
            => await _context.Set<Recipe>()
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.Tags)
                .ToListAsync();

        public async Task<Recipe?> GetAsync(int id) 
            => await _context.Set<Recipe>()
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.Tags)
                .FirstOrDefaultAsync(recipe => recipe.Id == id);

        public async Task<bool> DoesRecipeExistAsync(int id) => await GetAsync(id) != null;
    }
}

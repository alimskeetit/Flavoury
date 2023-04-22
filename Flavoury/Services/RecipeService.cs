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

        public async Task<ICollection<Recipe>> GetAllAsync(bool asTracking = false)
            => asTracking ?
                await _context.Set<Recipe>()
                    .Include(recipe => recipe.Ingredients)
                    .Include(recipe => recipe.Tags)
                    .AsTracking()
                    .ToListAsync() :
                await _context.Set<Recipe>()
                    .Include(recipe => recipe.Ingredients)
                    .Include(recipe => recipe.Tags)
                    .AsNoTracking()
                    .ToListAsync();

        public async Task<Recipe?> GetAsync(int id, bool asTracking = false)
            => asTracking
                ? await _context.Set<Recipe>()
                    .Include(recipe => recipe.Ingredients)
                    .Include(recipe => recipe.Tags)
                    .AsTracking()
                    .FirstOrDefaultAsync(recipe => recipe.Id == id)
                : await _context.Set<Recipe>()
                    .Include(recipe => recipe.Ingredients)
                    .Include(recipe => recipe.Tags)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(recipe => recipe.Id == id);

        public async Task<bool> DoesRecipeExistAsync(int id) => await GetAsync(id, asTracking: false) != null;
    }
}

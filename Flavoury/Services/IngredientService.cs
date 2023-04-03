using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Flavoury.Services
{
    public class IngredientService: EntityService<Ingredient>
    {
        public IngredientService(AppDbContext context): base(context)
        {
            
        }

        public async Task<Ingredient?> GetAsync(int id) => await GetAsync(ingredient => ingredient.Id == id);

        public async Task<Ingredient?> GetByNameAsync(string name)
        {
            return await _context.Set<Ingredient>().FirstOrDefaultAsync(ingredient => ingredient.Name == name);
        }

        public async Task DeleteAsync(int id) => await DeleteAsync(ingredient => ingredient.Id == id);
    }
}

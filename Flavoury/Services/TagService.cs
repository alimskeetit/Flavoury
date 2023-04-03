using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Flavoury.Services
{
    public class TagService: EntityService<Tag>
    {
        public TagService(AppDbContext context): base(context)
        {

        }

        public async Task<Tag?> GetAsync(int id) => await GetAsync(tag => tag.Id == id);

        public async Task<Tag?> GetByNameAsync(string name, bool asTracking = false)
        {
            return await GetAsync(tag => tag.Name == name, asTracking);
        }

        public async Task DeleteAsync(int id) => await DeleteAsync(tag => tag.Id == id);
    }
}

using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Flavoury.Services
{
    public class EntityService<T>: IEntityService<T> where T: class
    {
        protected AppDbContext _context;

        public EntityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool asTracking = false)
        {
            var query = asTracking ? _context.Set<T>().AsTracking() : _context.Set<T>().AsNoTracking();
            
            if (predicate != null)
                query = query.Where(predicate);
            
            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asTracking = false)
        {
            var query = asTracking ? _context.Set<T>().AsTracking() : _context.Set<T>().AsNoTracking();

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await _context.Set<T>().Where(predicate).ToListAsync();
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

    }
}

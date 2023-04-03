using System.Linq.Expressions;

namespace Flavoury.Services
{
    public interface IEntityService<T>
    {
        Task<T> CreateAsync(T entity);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool asTracking = false);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asTracking = false);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Expression<Func<T, bool>> predicate);
    }
}
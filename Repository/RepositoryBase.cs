using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    internal class RepositoryBase<T>: IRepositoryBase<T> where T : class
    {
        protected AppDbContext _context { get; set; }

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }


        public void Create(T entity) => _context.Set<T>().Add(entity);

        public IQueryable<T> FindAll() => _context.Set<T>().AsNoTracking();
        
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            _context.Set<T>().Where(expression).AsNoTracking();

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);
    }
}

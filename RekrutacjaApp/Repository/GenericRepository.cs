using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using RekrutacjaApp.Data;

namespace RekrutacjaApp.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _db;

        protected GenericRepository(ApplicationDbContext context)
        {
            _dbContext = context;
            _db = _dbContext.Set<T>();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetAll(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null)
        {
            IQueryable<T> query = _db;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().ToListAsync();

        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public async Task Update(T entity)
        {
            _db.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}

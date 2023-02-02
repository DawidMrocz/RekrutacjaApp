using System.Linq.Expressions;

namespace RekrutacjaApp.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(Expression<Func<T,bool>> expression,List<string> includes = null);
        Task<List<T>> GetAll(
            List<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>,IOrderedQueryable<T>> orderedBy = null,
            List<string> includes = null
            );
        Task Add(T entity);
        Task Delete(int id);
        void Update(T entity);
    }
}

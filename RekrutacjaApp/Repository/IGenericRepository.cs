using MyWebApplication.Dtos;
using System.Linq.Expressions;

namespace RekrutacjaApp.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAll();
        //Task<List<TEntity>> GetAll(object? query);

        //Task<List<TEntity>> GetAll(
        //    Expression<Func<TEntity, bool>> expression = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //    List<string> includes = null
        //);

        Task<TEntity> GetById(int id);
        void Create(TEntity obj);
        void Update(TEntity obj,int id);
        void Delete(int obj);
    }
}

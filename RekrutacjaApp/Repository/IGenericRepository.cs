using MyWebApplication.Dtos;

namespace RekrutacjaApp.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAll(object queryParams);
        Task<TEntity> GetById(int id);
        void Create(TEntity obj);
        void Update(TEntity obj,int id);
        void Delete(int obj);
    }
}

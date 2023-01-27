using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        Task Save();
    }
}

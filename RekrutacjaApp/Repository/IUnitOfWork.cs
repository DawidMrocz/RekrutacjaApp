using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UserRepository { get; }
        void Save();
    }
}

using RekrutacjaApp.Models;

namespace RekrutacjaApp.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task Save();
    }
}

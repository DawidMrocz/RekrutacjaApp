using RekrutacjaApp.Data;
using RekrutacjaApp.Models;

namespace RekrutacjaApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IUserRepository Users { get; }
        public UnitOfWork(ApplicationDbContext dbContext,
                            IUserRepository jobs)
        {
            _dbContext = dbContext;
            Users = jobs;
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

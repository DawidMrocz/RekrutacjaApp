using RekrutacjaApp.Data;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using System;

namespace RekrutacjaApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<User> _users;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            //_userRepository = new GenericRepository<User>(_context);

        }

        public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_context);
        //public IGenericRepository<User> UserRepository
        //{
        //    get
        //    {
        //        if (this.userRepository == null)
        //        {
        //            this.userRepository = new GenericRepository<User>(_context);
        //        }
        //        return userRepository;
        //    }
        //}

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();

            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}

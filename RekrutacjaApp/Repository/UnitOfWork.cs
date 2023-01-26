using RekrutacjaApp.Data;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using System;

namespace RekrutacjaApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGenericRepository<UserDto> userRepository;

        public IGenericRepository<UserDto> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<UserDto>(_context);
                }
                return userRepository;
            }
        }

        public async void Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);

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

using Microsoft.EntityFrameworkCore;
using RekrutacjaApp.Data;
using System;

namespace RekrutacjaApp.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public readonly ApplicationDbContext _context;
        public readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        //public virtual async Task<List<TEntity>> GetAll(object? queryParams)
        //{
        //    //Musze tutaj dodac queryparamasy
        //    return _dbSet.ToList();
        //}
        public virtual async Task<List<TEntity>> GetAll()
        {
            //Musze tutaj dodac queryparamasy
            return _dbSet.ToList();
        }
        public virtual async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        public virtual async void Delete(int id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);
            Delete(entityToDelete);
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }
        public virtual void Update(TEntity entityToUpdate,int id)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}

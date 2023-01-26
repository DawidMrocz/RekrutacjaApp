using Microsoft.EntityFrameworkCore;
using RekrutacjaApp.Data;
using System;

namespace RekrutacjaApp.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationDbContext _context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            context = _context;
            this.dbSet = context.Set<TEntity>();
        }
        public virtual async Task<List<TEntity>> GetAll(object queryParams)
        {
            return dbSet.ToList();
        }
        public virtual async Task<TEntity> GetById(int id)
        {
            return dbSet.Find(id);
        }
        public virtual void Create(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void Delete(int id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        public virtual void Update(TEntity entityToUpdate,int id)
        {
            dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}

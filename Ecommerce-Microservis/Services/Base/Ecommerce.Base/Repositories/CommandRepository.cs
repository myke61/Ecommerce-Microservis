using Ecommerce.Base.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Base.Repositories
{
    public class CommandRepository<T> : ICommandRepository<T> where T : class
    {
        private readonly DbContext _context;

        public CommandRepository(DbContext context)
        {
            _context = context; 
        }

        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry = await _context.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }

        public void Delete(T entity)
        {
            EntityEntry<T> entityEntry = _context.Remove(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public void Update(T entity) 
        {
            EntityEntry<T> entityEntry = _context.Update(entity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void DeleteById(Guid id) 
        {
            T entity = _context.Set<T>().Find(id);
            if(entity != null)
                _context.Set<T>().Remove(entity);
        }
    }
}

using Ecommerce.Base.Entities;
using Ecommerce.Base.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Base.Repositories
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private DbContext _dbContext {  get; set; } 
        private bool disposed = false;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (!this.disposed)
            {
                if (disposing)
                    _dbContext.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryRepository<T> GetQuery<T>() where T : BaseEntity
        {
            return new QueryRepository<T>(_dbContext);
        }

        public ICommandRepository<T> GetCommandRepository<T>() where T : BaseEntity
        {
            return new CommandRepository<T>(_dbContext);
            /*var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
            var type = types.First(x => typeof(CommandRepository<T>).IsAssignableFrom(x)) ?? throw new InvalidOperationException($"No matching type found for {typeof(CommandRepository<T>).Name}");
            var instance = Activator.CreateInstance(type, args: [_dbContext]) as CommandRepository<T>;
            return instance;*/
        }
    }
}

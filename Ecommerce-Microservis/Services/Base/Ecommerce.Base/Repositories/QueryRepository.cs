using Ecommerce.Base.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Base.Repositories
{
    public class QueryRepository<T> : IQueryRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        public QueryRepository(DbContext dbContext) 
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = CreateQuery(filter, include);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IList<T>> GetListAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = CreateQuery(filter, include);
            return await query.ToListAsync();
        }

        private IQueryable<T> CreateQuery(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null) 
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if(filter != null)
                query = query.Where(filter);
            if(include != null)
                query = include(query);
            return query;
        }
    }
}

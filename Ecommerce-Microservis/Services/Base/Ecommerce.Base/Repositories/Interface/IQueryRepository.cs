using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Ecommerce.Base.Repositories.Interface
{
    public interface IQueryRepository<T> : IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>,IIncludableQueryable<T,object>>? include = null);
        Task<IList<T>> GetListAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IList<T>> GetPagedListAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
    }
}

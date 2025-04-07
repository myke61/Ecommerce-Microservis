using Ecommerce.Base.Entities;

namespace Ecommerce.Base.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public IQueryRepository<T> GetQuery<T>() where T : BaseEntity;

        public ICommandRepository<T> GetCommandRepository<T>() where T : BaseEntity;

        public Task SaveAsync();
    }
}

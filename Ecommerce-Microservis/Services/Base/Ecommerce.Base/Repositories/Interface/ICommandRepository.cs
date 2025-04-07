namespace Ecommerce.Base.Repositories.Interface
{
    public interface ICommandRepository<T> : IRepository<T> where T : class
    {
        Task<bool> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteById(Guid id);
        Task<int> SaveAsync();
    }
}

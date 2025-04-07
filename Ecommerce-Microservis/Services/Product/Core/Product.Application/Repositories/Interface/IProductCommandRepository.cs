using Ecommerce.Base.Repositories.Interface;

namespace Product.Application.Repositories.Interface
{
    public interface IProductCommandRepository : ICommandRepository<Domain.Entities.Product>
    {
    }
}

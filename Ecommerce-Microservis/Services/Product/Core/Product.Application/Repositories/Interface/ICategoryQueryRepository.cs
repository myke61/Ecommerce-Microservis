using Ecommerce.Base.Repositories.Interface;
using Product.Domain.Entities;

namespace Product.Application.Repositories.Interface
{
    public interface ICategoryQueryRepository : IQueryRepository<Category>
    {
    }
}

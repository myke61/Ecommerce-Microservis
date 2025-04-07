using Ecommerce.Base.Repositories;
using Product.Application.Repositories.Interface;
using Product.Persistence.Context;

namespace Product.Persistence.Repository
{
    public class ProductCommandRepository : CommandRepository<Domain.Entities.Product>, IProductCommandRepository
    {
        public ProductCommandRepository(ProductDbContext context) : base(context)
        {
        }
    }
}

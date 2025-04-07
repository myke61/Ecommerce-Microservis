using Ecommerce.Base.Repositories;
using Product.Application.Repositories.Interface;
using Product.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Persistence.Repository
{
    public class ProductQueryRepository : QueryRepository<Domain.Entities.Product>, IProductQueryRepository
    {
        public ProductQueryRepository(ProductDbContext context) : base(context)
        {
        }
    }
}

using Ecommerce.Base.Repositories;
using Product.Application.Repositories.Interface;
using Product.Domain.Entities;
using Product.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Persistence.Repository
{
    public class CategoryQueryRepository : QueryRepository<Category>, ICategoryQueryRepository
    {
        public CategoryQueryRepository(ProductDbContext context) : base(context)
        {
        }
    }
}

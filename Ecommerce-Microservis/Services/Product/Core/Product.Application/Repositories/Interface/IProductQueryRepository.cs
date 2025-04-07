using Ecommerce.Base.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Repositories.Interface
{
    public interface IProductQueryRepository : IQueryRepository<Domain.Entities.Product>
    {
    }
}

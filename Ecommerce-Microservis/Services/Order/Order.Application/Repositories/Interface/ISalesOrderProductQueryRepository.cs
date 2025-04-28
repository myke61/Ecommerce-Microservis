using Ecommerce.Base.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Repositories.Interface
{
    public interface ISalesOrderProductQueryRepository : IQueryRepository<Domain.Entities.SalesOrderProduct>
    {
    }
}

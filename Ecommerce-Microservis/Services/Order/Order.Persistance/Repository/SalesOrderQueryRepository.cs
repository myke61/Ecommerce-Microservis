using Ecommerce.Base.Repositories;
using Order.Application.Repositories.Interface;
using Order.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Persistance.Repository
{
    public class SalesOrderQueryRepository : QueryRepository<Domain.Entities.SalesOrder>, ISalesOrderQueryRepository
    {
        public SalesOrderQueryRepository(OrderDbContext context) : base(context)
        {
        }
    }
}

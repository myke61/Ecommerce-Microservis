using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Requests.GetOrders
{
    public class GetOrdersRequest
    {
        public Guid UserId { get; set; }
    }
}

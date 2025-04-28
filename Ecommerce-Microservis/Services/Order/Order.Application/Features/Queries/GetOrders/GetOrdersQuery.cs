using MediatR;
using Order.Application.Responses.GetOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<GetOrdersResponse>
    {
        public Guid UserId { get; set; }
    }
}

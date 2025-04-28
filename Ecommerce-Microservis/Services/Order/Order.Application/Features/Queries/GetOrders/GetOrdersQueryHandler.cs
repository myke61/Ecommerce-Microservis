using Ecommerce.Base.Repositories.Interface;
using MassTransit.Initializers;
using MediatR;
using Order.Application.Responses.GetOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Queries.GetOrders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, GetOrdersResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetOrdersResponse> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            GetOrdersResponse response = new()
            {
                SalesOrders = []
            };
            var salesOrders = await _unitOfWork.GetQuery<Domain.Entities.SalesOrder>().GetListAsync(x => x.UserId == request.UserId);
            if (salesOrders != null)
            {
                foreach (var salesOrder in salesOrders)
                {
                    SalesOrderResponseDto salesOrderResponseDto = new()
                    {
                        Id = salesOrder.Id,
                        OrderStatus = salesOrder.OrderStatus,
                        TotalPrice = salesOrder.TotalPrice,
                        Products = []
                    };
                    var orderItems = await _unitOfWork.GetQuery<Domain.Entities.SalesOrderProduct>().GetListAsync(x => x.SalesOrderId == salesOrder.Id);
                    if (orderItems != null)
                    {
                        foreach (var orderItem in orderItems)
                        {
                            salesOrderResponseDto.Products.Add(new SalesOrderProductResponseDto
                            {
                                ProductId = orderItem.ProductId,
                                ProductName = orderItem.ProductName,
                                Price = orderItem.ProductPrice,
                                Quantity = orderItem.Quantity
                            });
                        }
                    }
                    response.SalesOrders.Add(salesOrderResponseDto);
                }
            }
            return response;
        }
    }
}

using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Order.Application.Responses.CreateOrder;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Command.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand,CreateOrderResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            SalesOrder salesOrder = new()
            {
                UserId = request.UserId,
                TotalPrice = request.TotalPrice,
                OrderStatus = request.OrderStatus,
                SalesOrderProduct = []
            };
            var response = await _unitOfWork.GetCommandRepository<SalesOrder>().AddAsync(salesOrder);
            if (!response)
            {
                return new CreateOrderResponse
                {
                    IsSuccess = false
                };
            }
            request.SalesOrderProductCommand.ForEach(async item =>
            {
                var salesOrderPro = new SalesOrderProduct
                {
                    SalesOrderId = salesOrder.Id,
                    SalesOrder = salesOrder,
                    ProductId = item.ProductId,
                    ProductPrice = item.UnitPrice,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                };
                await _unitOfWork.GetCommandRepository<SalesOrderProduct>().AddAsync(salesOrderPro);
            });
            
            await _unitOfWork.SaveAsync();
            return new CreateOrderResponse
            {
                IsSuccess = true
            };
        }
    }    
}

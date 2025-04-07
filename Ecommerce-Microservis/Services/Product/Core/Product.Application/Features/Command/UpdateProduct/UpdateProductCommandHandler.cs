using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Product.Application.Responses.UpdateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Features.Command.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product product = await unitOfWork.GetQuery<Domain.Entities.Product>().GetAsync(x => x.Id == request.Id) ?? throw new Exception("Product Not Found");
            product.Name = request.Name;
            product.Category = request.Category;    
            product.ImageURL = request.ImageURl;
            product.Price = request.Price;
            unitOfWork.GetCommandRepository<Domain.Entities.Product>().Update(product);
            await unitOfWork.SaveAsync();
            return new UpdateProductResponse
            {
                IsSuccess = true,
            };
        }
    }
}

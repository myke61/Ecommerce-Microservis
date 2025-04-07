using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Product.Application.Requests.DeleteProduct;
using Product.Application.Responses.DeleteProduct;

namespace Product.Application.Features.Command.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeleteProductResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<DeleteProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product product = await unitOfWork.GetQuery<Domain.Entities.Product>().GetAsync(x => x.Id == request.Id) ?? throw new Exception("Product Not Found");
            product.IsDeleted = true;
            unitOfWork.GetCommandRepository<Domain.Entities.Product>().Update(product);
            await unitOfWork.SaveAsync();
            return new DeleteProductResponse
            {
                IsSuccess = true
            };
        }
    }
}

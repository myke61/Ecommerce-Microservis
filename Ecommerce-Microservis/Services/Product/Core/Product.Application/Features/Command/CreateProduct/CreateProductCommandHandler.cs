using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Product.Application.Responses.CreateProduct;

namespace Product.Application.Features.Command.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product product = new(request.Code,request.Name,request.Category,request.ImageURL,request.Price);
            var response = new CreateProductResponse
            {
                IsSuccess = await unitOfWork.GetCommandRepository<Domain.Entities.Product>().AddAsync(product)
            };
            await unitOfWork.SaveAsync();
            return response;
        }
    }
}

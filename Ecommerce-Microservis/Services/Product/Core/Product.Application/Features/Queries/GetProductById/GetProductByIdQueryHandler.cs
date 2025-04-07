using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Product.Application.Responses.GetProductById;

namespace Product.Application.Features.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.GetQuery<Domain.Entities.Product>().GetAsync(p => p.Id == query.Id);
            return entity.Map();
        }
    }
}

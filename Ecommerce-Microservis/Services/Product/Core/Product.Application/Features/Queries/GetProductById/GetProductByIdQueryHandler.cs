using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Product.Application.Responses.GetProductById;
using System.Linq.Expressions;

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
            Func<IQueryable<Domain.Entities.Product>, IIncludableQueryable<Domain.Entities.Product, object>> include = query =>
            query.Include(p => p.Brand)
                 .Include(p => p.Category)
                 .Include(p => p.Images)
                 .Include(p => p.Variants);

            Expression<Func<Domain.Entities.Product, bool>> filter = p => p.Id == query.Id;
            var entity = await _unitOfWork.GetQuery<Domain.Entities.Product>().GetAsync(filter,include);
            return entity.Map();
        }
    }
}

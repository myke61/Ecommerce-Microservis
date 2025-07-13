using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Product.Application.Responses.GetProductById;
using System.Linq.Expressions;
using Product.Domain.Entities;

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
            Func<IQueryable<ProductVariant>, IIncludableQueryable<ProductVariant, object>> include = query =>
            query.Include(p => p.Product)
                 .ThenInclude(p => p.Images)
                 .Include(p => p.Product.Category)
                    .Include(p => p.Product.Brand);

            Expression<Func<ProductVariant, bool>> filter = p => p.Id == query.Id;
            var entity = await _unitOfWork.GetQuery<ProductVariant>().GetAsync(filter,include);
            return entity.Map();
        }
    }
}

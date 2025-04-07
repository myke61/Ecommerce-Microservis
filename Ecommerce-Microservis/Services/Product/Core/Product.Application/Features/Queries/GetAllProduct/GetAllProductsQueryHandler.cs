using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Product.Application.Responses.GetAllProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Features.Queries.GetAllProduct
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, GetAllProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllProductResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var totalProductCount = _unitOfWork.GetQuery<Domain.Entities.Product>().GetListAsync().Result.Count();

            List<Domain.Entities.Product> products = (List<Domain.Entities.Product>)await _unitOfWork.GetQuery<Domain.Entities.Product>().GetListAsync();

            return new()
            {
                Products = products
            };
        }
    }
}

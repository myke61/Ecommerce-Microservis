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
            var repository = _unitOfWork.GetQuery<Domain.Entities.Product>();

            int totalCount = await repository.CountAsync();

            var products = await repository.GetPagedListAsync(
                page: request.Page,
                pageSize: request.PageSize
            );

            return new GetAllProductResponse
            {
                Products = products.Select(p => new Domain.Entities.Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                    Slug = p.Slug,
                    IsDeleted = p.IsDeleted,
                    BrandId = p.BrandId,
                    Brand = p.Brand,
                    CategoryId = p.CategoryId,
                    Category = p.Category,
                    Variants = p.Variants,
                    Images = p.Images,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate
                }).ToList(),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}

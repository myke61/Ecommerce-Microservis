using AutoMapper;
using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Responses.GetAllProduct;
using Product.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var repository = _unitOfWork.GetQuery<Domain.Entities.ProductVariant>();

            Expression<Func<ProductVariant, bool>> filter = pv =>
                (string.IsNullOrEmpty(request.Name) || pv.Product.Name.Contains(request.Name) || pv.Product.Description.Contains(request.Name)) &&
                (string.IsNullOrEmpty(request.Category) || pv.Product.Category.Name == request.Category) &&
                (!request.MinPrice.HasValue || pv.Price >= request.MinPrice.Value) &&
                (!request.MaxPrice.HasValue || pv.Price <= request.MaxPrice.Value);

            var variants = await repository.GetListAsync(filter, include: query =>
                query.Include(pv => pv.Product)
                     .ThenInclude(p => p.Category)
                     .Include(pv => pv.Product.Brand)
                     //.Include(pv => pv.Product.Variants)
                     .Include(pv => pv.Product.Images)
            );

            // Sıralama
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                variants = request.SortBy.ToLower() switch
                {
                    "name" => [.. variants.OrderBy(pv => pv.Product.Name)],
                    "price" => [.. variants.OrderBy(pv => pv.Price)],
                    _ => variants.OrderByDescending(pv => pv.Product.CreatedDate).ToList()
                };
            }
            else
            {
                variants = [.. variants.OrderByDescending(pv => pv.Product.CreatedDate)];
            }

            // Gruplama (bir Product sadece bir kez listelensin)
            /*var groupedProducts = variants
                .GroupBy(pv => pv.Product.Id)
                .Select(g => g.First().Product)
                .ToList();*/

            int totalCount = variants.Count;

            // Sayfalama
            var pagedProductVariants = variants
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            List<ProductVariantDto> dtoProducts = [];
            foreach (var variant in pagedProductVariants)
            {
                ProductVariantDto pro = new()
                {
                    Id = variant.Id,
                    Name = variant.Product.Name,
                    Code = variant.Product.Code,
                    Description = variant.Product.Description,
                    Slug = variant.Product.Slug,
                    Price = variant.Price,
                    Brand = new BrandDto
                    {
                        Id = variant.Product.BrandId,
                        Name = variant.Product.Brand.Name
                    },
                    Category = new CategoryDto
                    {
                        Id = variant.Product.CategoryId,
                        Name = variant.Product.Category.Name
                    },
                    Images = [.. variant.Product.Images.Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        Url = i.ImageUrl,
                        IsMain = i.IsMain
                    })],
                    CreatedDate = variant.CreatedDate,
                    UpdatedDate = variant.UpdatedDate
                };
                dtoProducts.Add(pro);
            }

            return new GetAllProductResponse
            {
                ProductVariants = dtoProducts,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}

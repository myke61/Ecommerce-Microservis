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
                     .Include(pv => pv.Product.Variants)
                     .Include(pv => pv.Product.Images)
            );

            // Sıralama
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                variants = request.SortBy.ToLower() switch
                {
                    "name" => variants.OrderBy(pv => pv.Product.Name).ToList(),
                    "price" => variants.OrderBy(pv => pv.Price).ToList(),
                    _ => variants.OrderByDescending(pv => pv.Product.CreatedDate).ToList()
                };
            }
            else
            {
                variants = variants.OrderByDescending(pv => pv.Product.CreatedDate).ToList();
            }

            // Gruplama (bir Product sadece bir kez listelensin)
            var groupedProducts = variants
                .GroupBy(pv => pv.Product.Id)
                .Select(g => g.First().Product)
                .ToList();

            int totalCount = groupedProducts.Count;

            // Sayfalama
            var pagedProducts = groupedProducts
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            List<ProductDto> dtoProducts = [];
            foreach (var product in pagedProducts)
            {
                ProductDto pro = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Code = product.Code,
                    Description = product.Description,
                    Slug = product.Slug,
                    Brand = new BrandDto
                    {
                        Id = product.Brand.Id,
                        Name = product.Brand.Name
                    },
                    Category = new CategoryDto
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name
                    },
                    Variants = [.. product.Variants.Select(v => new ProductVariantDto
                    {
                        Id = v.Id,
                        Sku = v.Sku,
                        Price = v.Price
                    })],
                    Images = [.. product.Images.Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        Url = i.ImageUrl,
                        IsMain = i.IsMain
                    })],
                    CreatedDate = product.CreatedDate,
                    UpdatedDate = product.UpdatedDate
                };
                dtoProducts.Add(pro);
            }

            return new GetAllProductResponse
            {
                Products = dtoProducts,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}

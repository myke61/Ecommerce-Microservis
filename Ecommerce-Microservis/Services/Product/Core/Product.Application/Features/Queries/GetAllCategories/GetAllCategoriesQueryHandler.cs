using Ecommerce.Base.Repositories.Interface;
using MediatR;
using Product.Application.Responses.GetAllCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Features.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, GetAllCategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllCategoryResponse> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetQuery<Domain.Entities.Category>();
            var categories = await repository.GetListAsync();
            var response = new GetAllCategoryResponse
            {
                Categories = [.. categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    Description = c.Description
                })]
            };
            return response;
        }
    }
}

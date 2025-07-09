using MediatR;
using Product.Application.Responses.GetAllCategory;

namespace Product.Application.Features.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<GetAllCategoryResponse>
    {
    }
}

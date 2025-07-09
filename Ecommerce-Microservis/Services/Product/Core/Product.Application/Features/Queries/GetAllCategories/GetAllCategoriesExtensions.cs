using Product.Application.Responses.GetAllCategory;

namespace Product.Application.Features.Queries.GetAllCategories
{
    public static class GetAllCategoriesExtensions
    {
        public static GetAllCategoryResponse Map(this List<CategoryDto> categories)
        {
            return new GetAllCategoryResponse
            {
                Categories = categories,
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Responses.GetAllCategory
{
    public class GetAllCategoryResponse
    {
        public List<CategoryDto> Categories { get; set; }
    }

    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
    }
}

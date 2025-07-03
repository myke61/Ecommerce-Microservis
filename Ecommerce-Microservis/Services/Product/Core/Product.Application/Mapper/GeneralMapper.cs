using AutoMapper;
using Product.Application.Features.Queries.GetAllProduct;
using Product.Application.Features.Queries.GetProductById;
using Product.Application.Requests.GetAllProduct;
using Product.Application.Requests.GetProductById;

namespace Product.Application.Mapper
{
    public class GeneralMapper : Profile
    {
        public GeneralMapper() 
        {
            CreateMap<GetProductByIdRequest, GetProductByIdQuery>().ReverseMap();
            CreateMap<GetAllProductRequest, GetAllProductsQuery>().ReverseMap();
        }
    }
}

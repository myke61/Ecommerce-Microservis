using AutoMapper;
using Product.Application.Features.Command.CreateProduct;
using Product.Application.Features.Command.DeleteProduct;
using Product.Application.Features.Command.UpdateProduct;
using Product.Application.Features.Queries.GetAllProduct;
using Product.Application.Features.Queries.GetProductById;
using Product.Application.Requests.CreateProduct;
using Product.Application.Requests.DeleteProduct;
using Product.Application.Requests.GetAllProduct;
using Product.Application.Requests.GetProductById;
using Product.Application.Requests.UpdateProduct;

namespace Product.Application.Mapper
{
    public class GeneralMapper : Profile
    {
        public GeneralMapper() 
        {
            CreateMap<CreateProductRequest, CreateProductCommand>().ReverseMap();
            CreateMap<UpdateProductRequest, UpdateProductCommand>().ReverseMap();
            CreateMap<DeleteProductRequest, DeleteProductCommand>().ReverseMap();
            CreateMap<GetProductByIdRequest, GetProductByIdQuery>().ReverseMap();
            CreateMap<GetAllProductRequest, GetAllProductsQuery>().ReverseMap();
        }
    }
}

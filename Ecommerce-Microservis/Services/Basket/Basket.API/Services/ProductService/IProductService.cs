using Basket.API.DTOs.Product;

namespace Basket.API.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductById(Guid productId);
    }
}

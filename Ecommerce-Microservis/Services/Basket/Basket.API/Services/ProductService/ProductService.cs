using Basket.API.DTOs.Product;
using Newtonsoft.Json;

namespace Basket.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient client;

        public ProductService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<ProductDTO> GetProductById(Guid productId)
        {
            var response = await client.GetAsync($"/api/product/{productId}");
            var jsonContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductDTO>(jsonContent);
        }
    }
}

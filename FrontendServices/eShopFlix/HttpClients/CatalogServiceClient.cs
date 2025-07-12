using eShopFlix.Models;
using System.Text.Json;

namespace eShopFlix.HttpClients
{
    public class CatalogServiceClient
    {
        private readonly HttpClient _httpClient;
        public CatalogServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            var response = await _httpClient.GetAsync("catalog/GetAllProducts");
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<ProductModel>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? Enumerable.Empty<ProductModel>();
            }
            else
            {
                throw new Exception($"Error fetching products: {response.ReasonPhrase}");
            }
        }
        public async Task<ProductModel> GetProductById(int productId)
        {
            var response = await _httpClient.GetAsync($"catalog/GetProductById/{productId}");
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductModel>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new ProductModel();
            }
            else
            {
                throw new Exception($"Error fetching product with ID {productId}: {response.ReasonPhrase}");
            }
        }
    }
}

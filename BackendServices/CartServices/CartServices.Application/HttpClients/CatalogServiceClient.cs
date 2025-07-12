using CartServices.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CartServices.Application.HttpClients
{
    public class CatalogServiceClient
    {
        private readonly HttpClient _httpClient;
        public CatalogServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<ProductDTO>> GetByIdsAsync(int[] ints)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(ints),
                Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Product/GetProductsByIds/", content);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                {
                    return JsonSerializer.Deserialize<IEnumerable<ProductDTO>>(data, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            return null;
        }
    }
}

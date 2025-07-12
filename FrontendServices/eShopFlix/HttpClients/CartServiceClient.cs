using eShopFlix.Models;
using System.Text;
using System.Text.Json;

namespace eShopFlix.HttpClients
{
    public class CartServiceClient
    {
        private readonly HttpClient _httpClient;
        public CartServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CartModel> GetUserCartAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"/cart/GetUserCart/{userId}");
            //response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseBody))
                {
                    return JsonSerializer.Deserialize<CartModel>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            return null;
        }

        public async Task<int> DeleteCartItemAsync(int cartId, int itemId)
        {
            int response = await _httpClient.DeleteFromJsonAsync<int>($"/cart/DeleteItem/{itemId}/{cartId}");
            return response;
        }

        public async Task<int> UpdateQuantityAsync(int cartId, int itemId, int quantity)
        {
            var status = await _httpClient.GetFromJsonAsync<int>($"/cart/UpdateQuantity/{cartId}/{itemId}/{quantity}");
            return status;
        }

        public async Task<int> GetCartItemCountAsync(int userId)
        {
            var counter = await _httpClient.GetFromJsonAsync<int>($"/cart/GetCartItemCount/{userId}");
            return counter;
        }
        public async Task<CartModel> AddToCartAsync(CartItemModel cartItem, int userId)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(cartItem), Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync($"/cart/AddToCart/{userId}", content);
            if (response.IsSuccessStatusCode)
            {
                var cart = await response.Content.ReadFromJsonAsync<CartModel>();
                return cart;
            }
            return null;
        }
        public async Task<CartModel> GetCartAsync(int cartId)
        {
            var response = await _httpClient.GetFromJsonAsync<CartModel>($"/cart/GetCart/{cartId}");
            return response ?? null;
        }
        public async Task<bool> MakeCartInActiveAsync(int cartId)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"/cart/MakeInActive/{cartId}");
        }
    }
}

using eShopFlix.Message;
using eShopFlix.Models;

namespace eShopFlix.HttpClients
{
    public class OrderServiceClient
    {
        HttpClient _httpClient;
        public OrderServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            var response = await _httpClient.GetAsync("order/GetAllOrders");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<OrderModel>>();
            }
            return null;
        }
        public async Task<OrderModel> GetOrderByIdAsync(Guid orderId)
        {
            var response = await _httpClient.GetAsync($"order/GetOrderById/{orderId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderModel>();
            }
            return null;
        }
        public async Task<List<OrderModel>> GetOrdersByUserAsync(long userId)
        {
            var response = await _httpClient.GetAsync($"order/GetOrdersByUser/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<OrderModel>>();
            }
            return null;
        }
        public async Task<bool> SaveOrderAsync(OrderModel order, long cartId)
        {
            var response = await _httpClient.PostAsJsonAsync("order/SaveOrder", order);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> AcceptOrderAsync(Guid orderId, DateTime acceptDateTime)
        {
            var response = await _httpClient.PostAsJsonAsync($"order/AcceptOrder/{orderId}", acceptDateTime);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            var response = await _httpClient.DeleteAsync($"order/DeleteOrder/{orderId}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CreateOrderAsync(OrderMessage message)
        {
            var response = await _httpClient.PostAsJsonAsync("order/CreateOrderAsync", message);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

    }
}

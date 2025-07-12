using eShopFlix.Models;

namespace eShopFlix.HttpClients
{
    public class PaymentServiceClient
    {
        private readonly HttpClient _httpClient;
        public PaymentServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateOrderAsync(RazorPayOrderModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("payment/CreateOrder", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        public async Task<bool> SavePaymentDetailAsync(TransactionModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("payment/SavePaymentDetails", model);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<string> VerifyPayment(PaymentConfirmModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("payment/VerifyPayment", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
}

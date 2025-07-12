using eShopFlix.Models;
using System.Text;
using System.Text.Json;

namespace eShopFlix.HttpClients
{
    public class AuthServiceClient
    {
        HttpClient _httpClient;
        public AuthServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserModel> LoginAsync(LoginModel loginModel)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("auth/login", content);
            if(httpResponseMessage.IsSuccessStatusCode)
            {
                string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseContent))
                {
                    UserModel user = JsonSerializer.Deserialize<UserModel>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return user;
                }
            }
            return null;
            //var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
            //if (response.IsSuccessStatusCode)
            //{
            //    return await response.Content.ReadFromJsonAsync<UserModel>();
            //}
            //else
            //{
            //    throw new Exception("Login failed");
            //}
        }

        public async Task<bool> RegisterAsync(SignupModel model)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("auth/Signup", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}

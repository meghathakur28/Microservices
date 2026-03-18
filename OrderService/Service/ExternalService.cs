using OrderService.DTOs;
using System.Text.Json;

namespace OrderService.Service
{
    public class ExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ExternalService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<ProductDto> GetProduct(int productId)
        {
            var baseUrl = _config["Services:ProductService"];
            var response = await _httpClient.GetAsync($"{baseUrl}/api/products/{productId}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductDto>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // 🔍 Get User
        public async Task<UserDto> GetUser(int userId)
        {
            var baseUrl = _config["Services:UserService"];
            var response = await _httpClient.GetAsync($"{baseUrl}/api/users/{userId}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDto>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> ProcessPayment(int orderId, decimal amount)
        {
            var baseUrl = _config["Services:PaymentService"];

            var paymentData = new
            {
                OrderId = orderId,
                Amount = amount
            };

            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/payment", paymentData);

            return response.IsSuccessStatusCode;
        }
    }
}

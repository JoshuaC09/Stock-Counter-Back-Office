using Newtonsoft.Json;
using StockCounterBackOffice.Models;
using System.Net.Http;
using System.Text;

namespace StockCounterBackOffice.Services
{
    public class ApiService : HttpServiceBase
    {
        public ApiService(HttpClient httpClient, TokenService tokenService)
            : base(httpClient, tokenService)
        {
        }

        public async Task<bool> SetConnectionStringAsync(string connectionString)
        {
            try
            {
                var _baseUrl = GlobalVariable.BaseAddress.ToString();
                var encodedConnectionString = System.Net.WebUtility.UrlEncode(connectionString);
                var content = new StringContent(JsonConvert.SerializeObject(new { ConnectionString = encodedConnectionString }), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}api/Database/SetConnectionString", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API call failed: {response.StatusCode}, {errorContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return false;
            }
        }

        public async Task InitInventoryAsync()
        {
            var baseUrl = GlobalVariable.BaseAddress.ToString();
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsync($"{baseUrl}api/Inventory/init", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task PostInventoryAsync()
        {
            var baseUrl = GlobalVariable.BaseAddress.ToString();
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsync($"{baseUrl}api/Inventory/post", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ExportedItem>> ExportInventoryAsync()
        {
            var baseUrl = GlobalVariable.BaseAddress.ToString();
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsync($"{baseUrl}api/Inventory/export", null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ExportedItem>>(content);
        }
    }
}

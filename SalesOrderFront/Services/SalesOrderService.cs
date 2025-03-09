using SalesOrderFront.Interfaces;
using SalesOrderFront.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace SalesOrderFront.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly HttpClient _http;

        public SalesOrderService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<SalesOrderModel>> GetSalesOrdersAsync(SalesOrderFilter filter)
        {
            try
            {
                var json = JsonSerializer.Serialize(filter);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync("api/SalesOrder/GetList", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<SalesOrderModel>>>();
                    return result?.Data ?? new List<SalesOrderModel>();
                }
                else
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ API Error: {response.StatusCode} - {errorText}");
                    return new List<SalesOrderModel>(); // ✅ Return empty list if API fails
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception: {ex.Message}");
                return new List<SalesOrderModel>(); // ✅ Prevent crash if API fails
            }
        }




        public async Task<ApiResponse<int?>> MaintainSalesOrder(SalesOrderModel order, string action)
        {
            var request = new { order.RecId, order.OrderNo, order.OrderDate, order.CustomerName, StringAction = action };
            var response = await _http.PostAsJsonAsync("api/SalesOrder/Maintain", request);
            return await response.Content.ReadFromJsonAsync<ApiResponse<int?>>();
        }

        public async Task<SalesOrderModel> GetRecordSalesOrder(int recId)
        {
            var response = await _http.PostAsJsonAsync("api/SalesOrder/GetRecord", recId);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<SalesOrderModel>>();
            return result?.Data;
        }
    }
}

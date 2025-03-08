using SalesCustomerFront.Interfaces;
using SalesCustomerFront.Models;
using System.Text.Json;

namespace SalesCustomerFront.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly HttpClient _http;

        public SalesOrderService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<SalesOrderModel>> GetSalesOrdersAsync()
        {
            var response = _http.GetStreamAsync("api/SalesOrder/GetList");
            return await JsonSerializer.DeserializeAsync<List<SalesOrderModel>>(await response);
        }

        public async Task<ApiResponse<int?>> MaintainSalesOrder(SalesOrderModel order, string action)
        {
            var request = new { order.RecId, order.OrderNo, order.OrderDate, order.CustomerName, StringAction = action };
            var response = await _http.PostAsJsonAsync("api/SalesOrder/Maintain", request);
            return await response.Content.ReadFromJsonAsync<ApiResponse<int?>>();
        }

        public async Task<SalesOrderModel> GetRecordSalesOrder(int recId)
        {
            var response = await _http.PostAsJsonAsync("api/SalesOrder/GetRecord", new { RecId = recId });
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<SalesOrderModel>>();
            return result?.Data;
        }
    }
}

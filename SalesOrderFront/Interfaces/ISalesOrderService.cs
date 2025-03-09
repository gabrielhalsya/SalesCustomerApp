using SalesOrderFront.Models;

namespace SalesOrderFront.Interfaces
{
    public interface ISalesOrderService
    {
        Task<List<SalesOrderModel>> GetSalesOrdersAsync(SalesOrderFilter filter);
        Task<ApiResponse<int?>> MaintainSalesOrder(SalesOrderModel order, string action);
        Task<SalesOrderModel> GetRecordSalesOrder(int recId);
    }
}

using SalesCustomerFront.Models;

namespace SalesCustomerFront.Interfaces
{
    public interface ISalesOrderService
    {
        Task<List<SalesOrderModel>> GetSalesOrdersAsync();
        Task<ApiResponse<int?>> MaintainSalesOrder(SalesOrderModel order, string action);
        Task<SalesOrderModel> GetRecordSalesOrder(int recId);
    }
}

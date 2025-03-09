using SalesOrderFront.Models;

namespace SalesOrderFront.Interfaces
{
    public interface ISalesOrderViewModel
    {
        List<SalesOrderModel> SalesOrders { get; }
        string ErrorMessage { get; }
        Task LoadSalesOrders(SalesOrderFilter filter);
        Task<bool> MaintainOrder(SalesOrderModel order, string action);
        Task<SalesOrderModel> GetRecordById(int recId);
    }
}

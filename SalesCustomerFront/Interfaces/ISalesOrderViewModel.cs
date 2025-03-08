using SalesCustomerFront.Models;

namespace SalesCustomerFront.Interfaces
{
    public interface ISalesOrderViewModel
    {
        List<SalesOrderModel> SalesOrders { get; }
        string ErrorMessage { get; }
        Task LoadSalesOrders();
        Task<bool> MaintainOrder(SalesOrderModel order, string action);
        Task<SalesOrderModel> GetRecordById(int recId);
    }
}

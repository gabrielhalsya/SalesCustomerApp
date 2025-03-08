using SalesCustomerAPI.Models;

namespace SalesCustomerAPI.Repositories
{
    public interface ISalesOrderRepo
    {
        Task<SalesOrderBase> GetRecordSalesOrder(int recId);
        Task<(int? RecId, List<string> ErrorMessages)> MaintainSalesOrder(SalesOrderMaintain request);
        Task<IEnumerable<SalesOrderBase>> GetListSalesOrder(SalesOrderBase request);
    }
}


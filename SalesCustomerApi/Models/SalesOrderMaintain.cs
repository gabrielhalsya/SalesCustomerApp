namespace SalesCustomerAPI.Models
{
    public class SalesOrderMaintain : SalesOrderBase
    {
        public int? RecId { get; set; } = 0;
        public string StringAction { get; set; } = "";
        public string OrderNo { get; set; } = "";
        public int CustomerId { get; set; } = 0;
        public string CustomerName { get; set; } = "";
    }
}

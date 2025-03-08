namespace SalesCustomerFront.Models
{
    public class SalesOrderModel
    {
        public int? RecId { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }  // yyyyMMdd format
        public string CustomerName { get; set; }
    }
}

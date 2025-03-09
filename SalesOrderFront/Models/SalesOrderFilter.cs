namespace SalesOrderFront.Models
{
    public class SalesOrderFilter
    {
        public string OrderDate { get; set; } // yyyyMMdd format for API
        public string Search { get; set; } // Search text input
    }
}

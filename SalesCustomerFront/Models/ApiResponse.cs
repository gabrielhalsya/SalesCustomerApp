namespace SalesCustomerFront.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public bool IsSuccess => Errors == null || Errors.Count == 0;
    }
}

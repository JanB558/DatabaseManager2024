namespace ASPWebApp.Models
{
    public class ApiResponse<T>
    {
        public T? Value { get; set; }
        public int StatusCode { get; set; }
    }
}

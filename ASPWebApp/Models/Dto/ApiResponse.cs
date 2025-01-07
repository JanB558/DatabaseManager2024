namespace ASPWebApp.Models.Dto
{
    public class ApiResponse<T>
    {
        public T? Value { get; set; }
        public int StatusCode { get; set; }
    }
}

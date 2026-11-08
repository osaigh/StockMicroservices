namespace StockMicroservices.API.Errors
{
    public class ErrorMessage
    {
        public string Message { get; set; } = string.Empty;

        public string? StackTrace { get; set; }
    }
}

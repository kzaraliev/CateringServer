namespace Catering.Core.Models.Error
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Details { get; set; }
        public string? TraceId { get; set; }
    }
}

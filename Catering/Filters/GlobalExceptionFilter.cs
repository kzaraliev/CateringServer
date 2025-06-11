using Catering.Core.Models.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace Catering.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            int statusCode;
            string message;
            object details = null;

            // Log the exception details for internal debugging
            _logger.LogError(context.Exception, "An unhandled exception occurred during request processing.");

            switch (context.Exception)
            {
                case InvalidOperationException iopEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = iopEx.Message;
                    break;
                case ArgumentNullException anEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = anEx.Message;
                    break;
                case ArgumentOutOfRangeException aoorEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = aoorEx.Message;
                    break;
                case ArgumentException argEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = argEx.Message;
                    break;
                case KeyNotFoundException knfEx:
                    statusCode = StatusCodes.Status404NotFound;
                    message = knfEx.Message;
                    break;
                case UnauthorizedAccessException uaEx:
                    statusCode = StatusCodes.Status403Forbidden;
                    message = uaEx.Message;
                    break;
                case NotSupportedException nsEx:
                    // For operations that are not supported or implemented
                    statusCode = StatusCodes.Status501NotImplemented;
                    message = nsEx.Message;
                    break;
                case ValidationException valEx:
                    statusCode = StatusCodes.Status409Conflict;
                    message = valEx.Message;
                    break;
                case NotImplementedException niEx: // For methods that are not yet implemented
                    statusCode = StatusCodes.Status501NotImplemented;
                    message = niEx.Message;
                    break;
                case FormatException fEx:
                    // For issues where input data format is incorrect
                    statusCode = StatusCodes.Status400BadRequest;
                    message = $"Invalid data format: {fEx.Message}";
                    break;
                case OverflowException oEx: // For arithmetic operations that result in overflow
                    statusCode = StatusCodes.Status400BadRequest;
                    message = oEx.Message;
                    break;
                case DivideByZeroException dbzEx: // For division by zero errors
                    statusCode = StatusCodes.Status400BadRequest;
                    message = dbzEx.Message;
                    break;
                case InvalidCastException icEx: // For invalid type conversions
                    statusCode = StatusCodes.Status400BadRequest;
                    message = icEx.Message;
                    break;
                case TimeoutException tEx:
                    // When an operation times out, e.g., calling an external service
                    statusCode = StatusCodes.Status503ServiceUnavailable;
                    message = $"Service temporarily unavailable due to a timeout: {tEx.Message}";
                    break;
                case HttpRequestException hrEx:
                    // For errors when making HTTP requests to external services
                    statusCode = StatusCodes.Status502BadGateway; // Or specific status from hrEx.StatusCode if available
                    message = $"Failed to communicate with an external service: {hrEx.Message}";
                    break;
                // Database related exceptions (assuming Entity Framework Core or similar)
                case Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateEx:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "A database error occurred while saving changes.";
                    break;
                // Serialization/Deserialization errors (e.g., malformed JSON in request body)
                case System.Text.Json.JsonException jsonEx: // For System.Text.Json
                    statusCode = StatusCodes.Status400BadRequest;
                    message = $"Invalid JSON format in request body: {jsonEx.Message}";
                    break;
                case NullReferenceException _: // Don't expose internal details for this!
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "An unexpected internal error occurred.";
                    break;
                case TaskCanceledException _:
                case OperationCanceledException _:
                    // If the operation was explicitly cancelled, e.g., by client disconnect or timeout
                    statusCode = StatusCodes.Status400BadRequest; // Or 503 if server-side cancellation/timeout
                    message = "Request was cancelled.";
                    break;
                default:
                    // Catch-all for any unhandled exceptions
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "An internal server error occurred.";
                    break;
            }

            var errorResponse = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Details = details,
                TraceId = context.HttpContext.TraceIdentifier // Good for correlating logs
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}

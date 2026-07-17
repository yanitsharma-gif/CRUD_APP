using Microsoft.AspNetCore.Diagnostics;
namespace Practice
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = exception.Message
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true; // means "yes, I handled this exception"
        }
    }
}

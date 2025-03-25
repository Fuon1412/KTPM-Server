using System.Net;
using System.Text.Json;
using Server.Enums.ErrorCodes;

namespace Server.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            var errorId = Guid.NewGuid();
            var traceId = httpContext.TraceIdentifier;

            int statusCode;
            string message = "An unexpected error occurred.";
            int errorCode = (int)AuthErrorCode.UnknownError;

            // Xử lý các exception đặc biệt (AuthException, UserException)
            if (ex is AuthException authEx)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = authEx.Message;
                errorCode = (int)authEx.ErrorCode;
            }
            else if (ex is UserException userEx)
            {
                statusCode = (int)HttpStatusCode.BadRequest;  // Ví dụ: BadRequest cho UserException
                message = userEx.Message;
                errorCode = (int)userEx.ErrorCode;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
            }

            _logger.LogError(ex, "{ErrorId} | {TraceId} | ErrorCode: {ErrorCode} | Exception: {Message}",
                errorId, traceId, errorCode, ex.Message);

            var errorResponse = new
            {
                Id = errorId,
                TraceId = traceId,
                StatusCode = statusCode,
                ErrorCode = errorCode,
                Message = message
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(errorResponse);
            await httpContext.Response.WriteAsync(json);
        }
    }
}

namespace Presentation.Middleware
{
    public class LoggingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, ILogger<LoggingMiddleware> logger)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
                logger.LogWarning("Non-authorized call on API!");

            await next(context);
        }
    }
}

namespace Uply.Web.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);

            logger.LogInformation("{date} request {method} {Url} returns {StatusCode}",
                    DateTime.UtcNow.ToShortDateString(),
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            logger.LogError("There's an exception: {exception}", ex.ToString());
        }
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(
            this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}

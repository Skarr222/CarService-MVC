namespace CarService_MVC.Intranet.Middleware;

public class ExceptionLoggingMiddleware
{
    private readonly ILogger<ExceptionLoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Nieobsłużony wyjątek na ścieżce {Path} ({Method})",
                context.Request.Path, context.Request.Method);

            if (!context.Response.HasStarted) context.Response.Redirect("/Dashboard/Error");
        }
    }
}
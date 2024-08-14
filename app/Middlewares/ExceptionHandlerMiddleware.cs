using Microsoft.AspNetCore.Diagnostics;

namespace app.Middlewares;
public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) : IExceptionHandler
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An Exception has occurred: {Message}", exception.Message);
        var result = Results.Redirect("/errors");
        await result.ExecuteAsync(httpContext);
        return await ValueTask.FromResult(true);
    }
}

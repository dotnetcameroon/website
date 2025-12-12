using app.Notifications;
using MediatR;

namespace app.Middlewares;

public class DatabaseCheckMiddleware(IPublisher publisher) : IMiddleware
{
    private readonly IPublisher _publisher = publisher;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await _publisher.Publish(new RequestMade());
        await next(context);
    }
}

using app.infrastructure.Persistence;
using app.Notifications;
using MediatR;

namespace app.Handlers;

public class RequestMade_AwakeTheDatabase(IDbContext dbContext, ILogger<RequestMade_AwakeTheDatabase> logger) : INotificationHandler<RequestMade>
{
    private readonly IDbContext _dbContext = dbContext;
    private readonly ILogger<RequestMade_AwakeTheDatabase> _logger = logger;

    public async Task Handle(RequestMade notification, CancellationToken cancellationToken)
    {
        try
        {
            _ = await _dbContext.Database.CanConnectAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while checking the database connection.");
            return;
        }
    }
}

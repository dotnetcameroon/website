using app.domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace app.Data.Interceptors;
public class DomainEventsInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    private readonly IPublisher _publisher = publisher;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        PublishEvents(eventData.Context!).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await PublishEvents(eventData.Context!);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishEvents(DbContext dbContext)
    {
        var entities = dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToArray();

        var events = entities
            .SelectMany(e => e.DomainEvents)
            .ToArray();

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }

        await Parallel.ForAsync(0, events.Length, async (i, ct) =>
        {
            await _publisher.Publish(events[i], ct);
        });
    }
}

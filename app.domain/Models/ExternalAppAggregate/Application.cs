using app.domain.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace app.domain.Models.ExternalAppAggregate;
public class Application : Entity<Guid>, IAggregateRoot
{
    public Guid ClientId { get; private set; }
    public string ClientName { get; private set; } = string.Empty;
    public string ClientSecret { get; private set; } = string.Empty; // A random encoded string used to identify the client
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => [.. _domainEvents];

    // For EF Core
    private Application(){}

    private Application(Guid clientId, string clientName)
    {
        ClientId = clientId;
        ClientName = clientName;
    }

    public static Application Create(Guid clientId, string clientName)
    {
        return new Application(clientId, clientName);
    }

    public void UpdateClientSecret(string clientSecret, IPasswordHasher<Application> passwordHasher)
    {
        ClientSecret = passwordHasher.HashPassword(this, clientSecret);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

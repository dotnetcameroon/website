using app.Models.Common;

namespace app.Models.EventAggregate.Entities;
public class Partner : Entity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string Logo { get; private set; } = string.Empty;
    public string Website { get; private set; } = string.Empty;

    private Partner(
        Guid id,
        string name,
        string logo,
        string website) : base(id)
    {
        Name = name;
        Logo = logo;
        Website = website;
    }

    public static Partner Create(
        string name,
        string logo,
        string website)
    {
        return new Partner(
            Guid.NewGuid(),
            name,
            logo,
            website);
    }
}

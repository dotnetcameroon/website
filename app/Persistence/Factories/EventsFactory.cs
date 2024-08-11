using app.Models.EventAggregate;
using Bogus;
using EntityFrameworkCore.Seeder.Base;

namespace app.Persistence.Factories;
public class EventsFactory : Factory<Event>
{
    protected override Faker<Event> BuildRules()
    {
        return new Faker<Event>()
            .RuleFor(e => e.Title, x => x.Music.Genre())
            .RuleFor(e => e.Description, x => x.Lorem.Lines());
    }
}

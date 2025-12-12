using app.domain.Models.EventAggregate.ValueObjects;
using Bogus;
using EntityFrameworkCore.Seeder.Base;

namespace app.infrastructure.Persistence.Factories;

public class ScheduleFactory : Factory<EventSchedule>
{
    protected override Faker<EventSchedule> BuildRules()
    {
        return new Faker<EventSchedule>()
            .RuleFor(x => x.Start, f => f.Date.Future().ToUniversalTime())
            .RuleFor(x => x.End, (f, x) => f.Date.Between(x.Start, x.Start).ToUniversalTime())
            .RuleFor(x => x.IsAllDay, f => f.Random.Bool());
    }
}

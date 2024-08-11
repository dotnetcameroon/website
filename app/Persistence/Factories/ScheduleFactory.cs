using app.Models.EventAggregate.ValueObjects;
using Bogus;
using EntityFrameworkCore.Seeder.Base;

namespace app.Persistence.Factories;
public class ScheduleFactory : Factory<Schedule>
{
    protected override Faker<Schedule> BuildRules()
    {
        return new Faker<Schedule>()
            .RuleFor(x => x.Start, f => f.Date.Future())
            .RuleFor(x => x.End, (f, x) => f.Date.Between(x.Start, x.Start))
            .RuleFor(x => x.IsAllDay, f => f.Random.Bool());
    }
}

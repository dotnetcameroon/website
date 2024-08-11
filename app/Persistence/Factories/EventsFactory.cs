using app.Models.EventAggregate;
using app.Models.EventAggregate.Enums;
using Bogus;
using EntityFrameworkCore.Seeder.Base;

namespace app.Persistence.Factories;
public class EventsFactory : Factory<Event>
{
    protected override Faker<Event> BuildRules()
    {
        return new Faker<Event>()
            .RuleFor(e => e.Title, x => $"{x.Music.Genre()} {x.Music.Genre()}")
            .RuleFor(e => e.Description, x => x.Lorem.Lines())
            .RuleFor(e => e.Schedule, _ => new ScheduleFactory().Generate())
            .RuleFor(e => e.Type, x => x.Random.Enum<EventType>())
            .RuleFor(e => e.Status, x => x.Random.Enum<EventStatus>())
            .RuleFor(e => e.HostingModel, x => x.Random.Enum<EventHostingModel>())
            .RuleFor(e => e.RegistrationLink, x => x.Image.PicsumUrl())
            .RuleFor(e => e.ImageUrl, x => x.Image.PicsumUrl());
    }
}

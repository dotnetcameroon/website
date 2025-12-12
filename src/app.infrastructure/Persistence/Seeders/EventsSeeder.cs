using app.business.Persistence;
using app.domain.Models.EventAggregate;
using app.domain.Models.EventAggregate.Entities;
using app.domain.Models.EventAggregate.Enums;
using app.domain.Models.EventAggregate.ValueObjects;
using app.infrastructure.Persistence.Factories;
using EntityFrameworkCore.Seeder.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace app.infrastructure.Persistence.Seeders;

public class EventsSeeder(IDbContext dbContext, ILogger<EventsSeeder> logger, IUnitOfWork unitOfWork) : ISeeder
{
    private readonly IDbContext _dbContext = dbContext;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<EventsSeeder> _logger = logger;

    public async Task SeedAsync()
    {
        var partners = await _dbContext.Partners.FirstOrDefaultAsync(p => p.Name.Contains("Infinite Solutions"));
        var events = new EventsFactory().Generate(5);
        var @event = Event.Create(
            "Infinite Days",
            "Welcome to Infinite Days, a tech extravaganza where innovation meets inspiration! This event brings together tech enthusiasts, developers, entrepreneurs, and industry leaders for a day packed with cutting-edge insights, hands-on workshops, and dynamic discussions. From exploring the latest trends in technology to diving deep into specialized topics like entrepreneurship, human resources management, and .NET development, Infinite Days is your gateway to endless possibilities. Whether you’re looking to level up your skills, network with like-minded professionals, or simply immerse yourself in the tech world, Infinite Days offers something for everyone. Join us for a journey through the infinite landscape of technology, where the only limit is your imagination!",
            EventSchedule.Create(new DateTime(2023, 10, 16).ToUniversalTime(), null, true),
            EventType.Conference,
            EventStatus.Passed,
            EventHostingModel.InPerson,
            "https://github.com/dotnetcameroon/Infinite-Days/blob/main/docs/brand.png?raw=true",
            null,
            70,
            null,
            [],
            [partners]);

        var activity = Activity.Create(
            "Introduction to .NET",
            "Step into the world of .NET, a powerful framework that has revolutionized the development of modern applications. This introductory event is designed for aspiring developers and tech enthusiasts who want to explore the capabilities of .NET. Discover how .NET can help you build robust, scalable, and high-performance applications across various platforms. We’ll cover the essentials, from the basics of the framework to the latest features and tools that make development easier and faster. Whether you're just starting out or looking to expand your skills, this event will equip you with the knowledge to start your journey in .NET development.",
            Host.Create("Djoufson Che", "djouflegran@gmail.com", "https://avatars.githubusercontent.com/u/101910784?v=4"),
            ActivitySchedule.Create(new TimeOnly(14, 00, 00), new TimeOnly(16, 00, 00)),
            @event);

        var activity2 = Activity.Create(
            "Entrepreneurship and Human Resources management",
            "Unlock the secrets to building a successful business with the right people by your side. This event dives deep into the intersection of entrepreneurship and human resources, exploring how strategic HR management can drive your business to new heights. Whether you’re a startup founder or an established entrepreneur, learn how to attract, retain, and nurture the talent that will propel your vision forward. Gain insights from industry experts on leadership, team dynamics, and the best practices for creating a thriving workplace culture. Join us to transform your HR approach and fuel your entrepreneurial journey.",
            Host.Create("Cédric Noumbo", "cnoumbo@gmail.com"),
            ActivitySchedule.Create(new TimeOnly(12, 00, 00), new TimeOnly(14, 00, 00)),
            @event);

        @event.AddActivity(activity2);
        @event.AddActivity(activity);
        await _dbContext.Events.AddRangeAsync([@event, .. events]);
        await _unitOfWork.SaveChangesAsync();
    }
}

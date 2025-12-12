using app.business.Services;
using app.Jobs.Base;
using Hangfire;

namespace app.Jobs;

public class MarkEventsAsPassed(IEventService eventService) : IJob
{
    private const string JobName = nameof(MarkEventsAsPassed);
    private readonly IEventService _eventService = eventService;

    public IEnumerable<JobDefinition> GetJobDefinitions()
    {
        yield return JobDefinition.Create(JobName, Cron.Daily(), () => _eventService.MarkPassedEventsAsync());
    }
}

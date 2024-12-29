using System.Reflection;
using app.Filters;
using app.Jobs.Base;
using Hangfire;

namespace app.Extensions;

public static class HangfireExtensions
{
    public static WebApplication MapHangfireJobs(this WebApplication app)
    {
        app.UseHangfireDashboard("/admin/jobs", new DashboardOptions
        {
            IsReadOnlyFunc = context => false,
            Authorization = [new HangfireAuthFilter()]
        });

        using var scope = app.Services.CreateScope();
        var jobs = scope.ServiceProvider.GetServices<IJob>();
        foreach (var job in jobs)
        {
            foreach (var jobDefinition in job.GetJobDefinitions())
            {
                RecurringJob.AddOrUpdate(jobDefinition.Name, jobDefinition.MethodCall, jobDefinition.Cron);
            }
        }

        return app;
    }

    public static IServiceCollection AddJobsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var jobs = assembly.GetTypes()
            .Where(t => typeof(IJob).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var job in jobs)
        {
            services.AddScoped(typeof(IJob), job);
        }

        return services;
    }
}

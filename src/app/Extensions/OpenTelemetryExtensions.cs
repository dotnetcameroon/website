
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;

namespace app.Extensions;
public static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder WithOpenTelemetry(this WebApplicationBuilder builder)
    {

        builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(res => res.AddService("Website"))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                metrics.AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation();

                tracing.AddOtlpExporter();
            });

        return builder;
    }
}

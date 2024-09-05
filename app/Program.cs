using app.Components;
using app.Extensions;
using app.Filters;
using app.Middlewares;
using app.Services;
using EntityFrameworkCore.Seeder.Extensions;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices(builder.Configuration, builder.Environment);

var app = builder.Build();

if(await app.MapSeedCommandsAsync(args))
{
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler(new ExceptionHandlerOptions
    {
        ExceptionHandlingPath = "/errors"
    });
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseMiddleware<DatabaseCheckMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseAntiforgery();

app.MapHangfireJobs();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

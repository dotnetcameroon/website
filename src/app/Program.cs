using app.Api.Culture;
using app.Api.DebugMode;
using app.Api.Identity;
using app.Api.Projects;
using app.business.Services;
using app.Components;
using app.Extensions;
using EntityFrameworkCore.Seeder.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WithOpenTelemetry();
builder.Services.AddServices(builder.Configuration, builder.Environment);
// Register localization and point to Resources folder
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources"; // this folder will contain .resx files
});

var app = builder.Build();

app.EnsureDatabaseCreated();

if (await app.MapSeedCommandsAsync(args))
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
}

app.UseHttpsRedirection();

app.MapStaticAssets();

// configure supported cultures
var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("fr-FR")
    // add more here if needed
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures.ToList(),
    SupportedUICultures = supportedCultures.ToList()
};

// ensure the cookie provider uses the same cookie name as your links (AppCulture)
var cookieProvider = localizationOptions.RequestCultureProviders
    .OfType<CookieRequestCultureProvider>()
    .FirstOrDefault();

if (cookieProvider != null)
{
    cookieProvider.CookieName = "AppCulture";
}
else
{
    localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider { CookieName = "AppCulture" });
}

app.UseRequestLocalization(localizationOptions);

app.UseAuthentication();

app.UseAuthorization();

app.UseAntiforgery();

// app.MapHangfireJobs();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(app.client._Imports).Assembly);

app.MapProjectsApi();
app.MapIdentityApi();
app.MapDebugModeApi();
app.MapCultureApi();

app.Run();

//

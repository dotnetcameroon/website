using app.business.Persistence;
using app.business.Services;
using app.domain.Models.Identity;
using app.infrastructure.Options;
using app.infrastructure.Persistence;
using app.infrastructure.Persistence.Interceptors;
using app.infrastructure.Persistence.Repositories.Base;
using app.infrastructure.Services;
using app.Jobs.Base;
using app.Middlewares;
using app.shared.Utilities;
using EntityFrameworkCore.Seeder.Extensions;
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace app.Extensions;
public static class Extensions
{
    private const string SqlServer = "SqlServer";
    private const string HangfireSqlite = "HangfireSqlite";
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        if (environment.IsProduction())
        {
            services.AddApplicationInsightsTelemetry(configuration);
            services.AddExceptionHandler<ExceptionHandlerMiddleware>();
        }
        services.AddHangfire(cfg => cfg
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSQLiteStorage(configuration.GetConnectionString(HangfireSqlite)));

        // We currently use the memory cache because it's enough for our simple application
        // We will scale to a distributed Redis Cache if needed
        services.AddCacheManager();
        services.AddSingleton<CacheManager>();

        services.AddHangfireServer();
        services.AddJobsFromAssembly(typeof(IJob).Assembly);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFileDownloader, FileDownloader>();
        services.AddScoped<IFileUploader, FileUploader>();
        services.AddScoped<IFileManager, FileManager>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IPartnerService, PartnerService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<DomainEventsInterceptor>();
        services.AddSqlServer<AppDbContext>(configuration.GetConnectionString(SqlServer));
        services.AddScoped<IDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Extensions).Assembly));
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        services.Configure<CookiesOptions>(configuration.GetRequiredSection(CookiesOptions.SectionName));
        services.AddAuth(configuration);

        // Seeders
        services.ConfigureSeedersEngine();
        services.AddSeedersFromAssembly(typeof(AppDbContext).Assembly);
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration conf)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
            {
                var cookiesOptions = conf.GetRequiredSection(CookiesOptions.SectionName).Get<CookiesOptions>()!;
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.Cookie.SameSite = SameSiteMode.Unspecified;
                opt.SlidingExpiration = true;
                opt.LoginPath = cookiesOptions.LoginPath;
                opt.LogoutPath = cookiesOptions.LogoutPath;
                opt.AccessDeniedPath = cookiesOptions.AccessDeniedPath;
                opt.ClaimsIssuer = cookiesOptions.Issuer;
                opt.ExpireTimeSpan = TimeSpan.FromDays(cookiesOptions.ExpirationInDays);

                // Handle authentication failures
                opt.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.Headers.Location = string.Empty; // Clear the Location header to prevent redirection
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.AdminOnly, p =>
            {
                p.RequireRole(Roles.Admin);
            });

        return services;
    }
}

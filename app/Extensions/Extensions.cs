using app.Data.Interceptors;
using app.Middlewares;
using app.Models.Identity;
using app.Options;
using app.Persistence;
using app.Persistence.Impl;
using app.Persistence.Repositories.Base;
using app.Services;
using app.Services.Impl;
using app.Utilities;
using EntityFrameworkCore.Seeder.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace app.Extensions;
public static class Extensions
{
    private const string SqlServer = "SqlServer";
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
        if(environment.IsProduction())
        {
            services.AddApplicationInsightsTelemetry(configuration);
            services.AddExceptionHandler<ExceptionHandlerMiddleware>();
        }

        // services.AddHangfire(cfg => cfg
        //     .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        //     .UseSimpleAssemblyNameTypeSerializer()
        //     .UseRecommendedSerializerSettings()
        //     .UseSqlServerStorage(configuration.GetConnectionString(SqlServer)));

        // We currently use the memory cache because it's enough for our simple application
        // We will scale to a distributed Redis Cache if needed
        services.AddCacheManager();
        services.AddSingleton<CacheManager>();

        // services.AddHangfireServer();
        // services.AddJobsFromAssembly(typeof(Extensions).Assembly);
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IFileDownloader,FileDownloader>();
        services.AddScoped<IFileUploader,FileUploader>();
        services.AddScoped<IFileManager,FileManager>();
        services.AddScoped<IEventService,EventService>();
        services.AddScoped<IPartnerService,PartnerService>();
        services.AddScoped<IIdentityService,IdentityService>();
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
        services.AddSeedersFromAssembly(typeof(Extensions).Assembly);
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

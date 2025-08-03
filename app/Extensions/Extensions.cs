using System.Text;
using app.business.Persistence;
using app.business.Services;
using app.domain.Models.ExternalAppAggregate;
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
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace app.Extensions;
public static class Extensions
{
    private const string SqlServer = "SqlServer";
    private const string HangfireSqlite = "HangfireSqlite";
    private const string Sqlite = "Sqlite";
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        services.AddOpenTelemetry()
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

        if (environment.IsProduction())
        {
            services.AddApplicationInsightsTelemetry(configuration);
            services.AddExceptionHandler<ExceptionHandlerMiddleware>();
        }

        // services.AddHangfire(cfg => cfg
        //     .UseSimpleAssemblyNameTypeSerializer()
        //     .UseRecommendedSerializerSettings()
        //     .UseMemoryStorage());

        // services.AddHangfireServer();
        // services.AddJobsFromAssembly(typeof(IJob).Assembly);

        // We currently use the memory cache because it's enough for our simple application
        // We will scale to a distributed Redis Cache if needed
        services.AddCacheManager();
        services.AddSingleton<CacheManager>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFileUploader, FileUploader>();
        services.AddScoped<IFileManager, FileManager>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IFileDownloader, FileDownloader>();
        services.AddScoped<IPartnerService, PartnerService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IExternalAppService, ExternalAppService>();
        services.AddScoped<IPasswordHasher<Application>>(sp => new PasswordHasher<Application>());
        services.AddScoped<DomainEventsInterceptor>();
        services.AddSqlite<ProjectDbContext>(configuration.GetConnectionString(Sqlite));
        services.AddSqlServer<AppDbContext>(configuration.GetConnectionString(SqlServer));
        services.AddScoped<IDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Extensions).Assembly));
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        services.Configure<CookiesOptions>(configuration.GetRequiredSection(CookiesOptions.SectionName));
        services.Configure<JwtOptions>(configuration.GetRequiredSection(JwtOptions.SectionName));
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
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                var jwtOptions = conf.GetRequiredSection(JwtOptions.SectionName).Get<JwtOptions>()!;
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.AdminOnly, p =>
            {
                p.RequireRole(Roles.Admin);
            })
            .AddPolicy(Policies.JwtAuthOnly, p =>
            {
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                p.RequireAuthenticatedUser();
            });

        return services;
    }
}

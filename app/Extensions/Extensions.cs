using app.Data.Interceptors;
using app.Models.Identity;
using app.Options;
using app.Persistence;
using app.Persistence.Impl;
using app.Persistence.Repositories.Base;
using app.Services;
using app.Services.Impl;
using app.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace app.Extensions;
public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IEventService,EventService>();
        services.AddScoped<DomainEventsInterceptor>();
        services.AddSqlServer<AppDbContext>(configuration.GetConnectionString("SqlServer"));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Extensions).Assembly));
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddIdentityCore<User>();
        services.Configure<CookiesOptions>(configuration.GetRequiredSection(CookiesOptions.SectionName));
        services.AddAuth(configuration);
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

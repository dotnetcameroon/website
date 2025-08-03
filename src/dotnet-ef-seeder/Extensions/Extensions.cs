using System.Reflection;
using EntityFrameworkCore.Seeder.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Seeder.Extensions;

public static class Extensions
{
    #region RegisterSeeders
    public static IServiceCollection ConfigureSeedersEngine(this IServiceCollection services)
    {
        services.AddScoped<SeederEngine>();
        return services;
    }

    public static IServiceCollection AddSeeder<TSeeder>(this IServiceCollection services)
        where TSeeder : class, ISeeder
    {
        services.AddScoped<ISeeder, TSeeder>();
        return services;
    }

    public static IServiceCollection AddSeedersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var seederTypes = assembly.GetTypes()
            .Where(t => typeof(ISeeder).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var seederType in seederTypes)
        {
            services.AddScoped(typeof(ISeeder), seederType);
        }

        return services;
    }

    public static IServiceCollection AddSeedersFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            services.AddSeedersFromAssembly(assembly);
        }

        return services;
    }
    #endregion

    #region Seed Commands
    public static async Task<bool> MapSeedCommandsAsync(this IApplicationBuilder app, string[] args)
    {
        if (args.Length == 0)
        {
            return false;
        }

        if (!args[0].Equals("--seed", StringComparison.CurrentCultureIgnoreCase)
            && !args[0].Equals("-s", StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        }

        using var scope = app.ApplicationServices.CreateScope();
        var seederEngine = scope.ServiceProvider.GetRequiredService<SeederEngine>();
        var seeders = scope.ServiceProvider.GetServices<ISeeder>();
        if (args.Length == 1)
        {
            return await seederEngine.RunAsync(seeders.ToArray());
        }

        var seederNames = args.Skip(1).ToArray();
        return await seederEngine.RunAsync(seederNames, seeders.ToArray());
    }
    #endregion
}

using System.Reflection;
using System.Runtime.CompilerServices;
using Inflow.Shared.Abstractions.Dispatchers;
using Inflow.Shared.Abstractions.Time;
using Inflow.Shared.Infrastructure.Api;
using Inflow.Shared.Infrastructure.Commands;
using Inflow.Shared.Infrastructure.Dispatchers;
using Inflow.Shared.Infrastructure.Postgres;
using Inflow.Shared.Infrastructure.Queries;
using Inflow.Shared.Infrastructure.Time;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Inflow.Bootstrapper")]
namespace Inflow.Shared.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddModularInfrastructure(this IServiceCollection services, IList<Assembly> assemblies)
    {
        var disabledModules = new List<string>();

        using var scope = services.BuildServiceProvider().CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        foreach (var (key, value) in configuration.AsEnumerable())
        {
            if(!key.Contains(":module:enabled")) continue;
            if (!bool.Parse(value))
            {
                disabledModules.Add(key.Split(":")[0]);
            }
        }
        services
            .AddCommands(assemblies)
            .AddQueries(assemblies)
            .AddPostgres()
            .AddSingleton<IClock, UtcClock>()
            .AddSingleton<IDispatcher, InMemoryDispatcher>()
            .AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                var removedParts = new List<ApplicationPart>();
                foreach (var disabledModule in disabledModules)
                {
                    var parts = manager.ApplicationParts.Where(x => x.Name.Contains(disabledModule));
                    removedParts.AddRange(parts);
                }

                foreach (var removedPart in removedParts)
                {
                    manager.ApplicationParts.Remove(removedPart);

                }

                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });
        return services;
    }

    public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : class, new()
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetOptions<T>(sectionName);
    }
    
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }
}
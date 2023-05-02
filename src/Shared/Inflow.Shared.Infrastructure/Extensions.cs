using System.Runtime.CompilerServices;
using Inflow.Shared.Abstractions.Time;
using Inflow.Shared.Infrastructure.Commands;
using Inflow.Shared.Infrastructure.Postgres;
using Inflow.Shared.Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Inflow.Bootstrapper")]
namespace Inflow.Shared.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddModularInfrastructure(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddPostgres()
            .AddSingleton<IClock, UtcClock>();
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
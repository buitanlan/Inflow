using System.Runtime.CompilerServices;
using Inflow.Shared.Abstractions.Time;
using Inflow.Shared.Infrastructure.Commands;
using Inflow.Shared.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Inflow.Bootstrapper")]
namespace Inflow.Shared.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddModularInfrastructure(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddSingleton<IClock, UtcClock>();
        return services;
    }
}
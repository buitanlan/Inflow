using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Time;
using Inflow.Shared.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Commands;

internal static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandlers<>))));
        return services;
    }
}
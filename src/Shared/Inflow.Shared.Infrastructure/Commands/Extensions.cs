using System.Reflection;
using Inflow.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services, IList<Assembly> assemblies)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandlers<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
}
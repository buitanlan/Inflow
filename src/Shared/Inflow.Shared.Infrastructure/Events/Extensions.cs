using System.Reflection;
using Inflow.Shared.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Events;

public static class Extensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services, IList<Assembly> assemblies)
    {
        services.AddSingleton<IEventDispatcher, EventDispatcher>();


        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

                foreach (var @interface in interfaces)
                {
                    services.AddScoped(@interface, type);
                }
            }
        }

        return services;
    }
}

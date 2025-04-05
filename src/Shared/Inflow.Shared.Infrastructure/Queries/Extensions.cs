using System.Reflection;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services, IList<Assembly> assemblies)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => 
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

                foreach (var @interface in interfaces)
                {
                    services.AddScoped(@interface, type);
                }
            }
        }
        return services;
    }
}
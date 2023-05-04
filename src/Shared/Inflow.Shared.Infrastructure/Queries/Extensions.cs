using System.Reflection;
using Inflow.Shared.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services, IList<Assembly> assemblies)
    {
        services
            .AddSingleton<IQueryDispatcher, QueryDispatcher>()
            .Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        return services;
    }
}
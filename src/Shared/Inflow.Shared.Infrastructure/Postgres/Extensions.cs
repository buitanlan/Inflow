using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Postgres;

public static class Extensions
{
    internal static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        var options = services.GetOptions<PostgresOptions>("ConnectionStrings");
        services
            .AddSingleton(options)
            .AddHostedService<DbContextAppInitializer>();
        return services;
    }

    public static IServiceCollection AddPostgres<T>(this IServiceCollection services)
        where T : DbContext
    {
        var options = services.GetOptions<PostgresOptions>("ConnectionStrings");
        services.AddNpgsql<T>(options.DefaultConnection);
        return services;
    }
}
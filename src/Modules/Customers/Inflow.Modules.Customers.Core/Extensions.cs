using System.Runtime.CompilerServices;
using Inflow.Modules.Customers.Core.Clients;
using Inflow.Modules.Customers.Core.DAL;
using Inflow.Modules.Customers.Core.DAL.Repositories;
using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Inflow.Modules.Customers.Api")]
namespace Inflow.Modules.Customers.Core;

internal static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomersRepository>()
                .AddSingleton<IUserApiClient, UserApiClient>()
                .AddPostgres<CustomersDbContext>();

        return services;
    }
}

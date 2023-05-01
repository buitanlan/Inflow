using System.Runtime.CompilerServices;
using Inflow.Modules.Customers.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Inflow.Bootstrapper")]
namespace Inflow.Modules.Customers.Api;

internal static class Extensions
{
    public static IServiceCollection AddCustomersModule(this IServiceCollection services)
    {
        services.AddCore();
        return services;
    }

    public static IApplicationBuilder UseCustomerModule(this IApplicationBuilder app)
    {
        return app;
    }
}
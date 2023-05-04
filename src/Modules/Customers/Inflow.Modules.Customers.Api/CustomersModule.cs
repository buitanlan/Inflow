using System.Runtime.CompilerServices;
using Inflow.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Inflow.Bootstrapper")]
namespace Inflow.Modules.Customers.Api;

internal class CustomersModule : IModule
{
    public string Name { get; } = "Customers";
    public void Register(IServiceCollection services)
    {
        services.AddCors();
    }

    public void Use(IApplicationBuilder app)
    {
        throw new NotImplementedException();
    }
}
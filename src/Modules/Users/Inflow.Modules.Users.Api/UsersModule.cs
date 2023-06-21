using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Inflow.Modules.Users.Core;
using Inflow.Shared.Abstractions.Modules;

namespace Inflow.Modules.Users.Api;

internal class UsersModule : IModule
{
    public string Name { get; } = "Users";
        
    public IEnumerable<string> Policies { get; } = new[]
    {
        "users"
    };

    public void Register(IServiceCollection services)
    {
        services.AddCore();
    }
        
    public void Use(IApplicationBuilder app)
    {
        // app.UseModuleRequests()
        //     .Subscribe<GetUserByEmail, UserDetailsDto>("users/get",
        //         (query, serviceProvider, cancellationToken) =>
        //             serviceProvider.GetRequiredService<IQueryDispatcher>().QueryAsync(query, cancellationToken));
    }
}
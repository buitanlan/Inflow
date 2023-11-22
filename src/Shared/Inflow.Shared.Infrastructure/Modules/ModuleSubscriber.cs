using Inflow.Shared.Abstractions.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Modules;

internal sealed class ModuleSubscriber(IModuleRegistry moduleRegistry, IServiceProvider serviceProvider) : IModuleSubscriber
{
    public IModuleSubscriber Subscribe<TRequest, TResponse>(string path, Func<TRequest, IServiceProvider, CancellationToken, Task<TResponse>> action)
        where TRequest : class
        where TResponse : class
    {
        var registration = new ModuleRequestRegistration(typeof(TRequest), typeof(TResponse),
            async (request, cancellationToken) =>
            {
                using var scope = serviceProvider.CreateScope();
                return await action((TRequest)request, scope.ServiceProvider, cancellationToken);
            });

        moduleRegistry.AddRequestAction(path, registration);
        return this;
    }
}

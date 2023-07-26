using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Events;

namespace Inflow.Shared.Infrastructure.Postgres.Decorators;

[Decorator]
public class TransactionalEventHandlerDecorator<T>(
    IEventHandler<T> handler,
    UnitOfWorkTypeRegistry unitOfWorkTypeTypeRegistry,
    IServiceProvider serviceProvider,
    ILogger<TransactionalEventHandlerDecorator<T>> logger) : IEventHandler<T> where T : class, IEvent
{
    public async Task HandleAsync(T @event, CancellationToken cancellationToken = default)
    {
        var unitOfWorkType = unitOfWorkTypeTypeRegistry.Resolve<T>();
        if (unitOfWorkType is null)
        {
            await handler.HandleAsync(@event, cancellationToken);
            return;
        }

        var unitOfWork = (IUnitOfWork) serviceProvider.GetRequiredService(unitOfWorkType);
        var unitOfWorkName = unitOfWorkType.Name;
        var name = @event.GetType().Name.Underscore();
        logger.LogInformation("Handling: {Name} using TX ({UnitOfWorkName})...", name, unitOfWorkName);
        await unitOfWork.ExecuteAsync(() => handler.HandleAsync(@event, cancellationToken));
        logger.LogInformation("Handled: {Name} using TX ({UnitOfWorkName})", name, unitOfWorkName);
    }
}

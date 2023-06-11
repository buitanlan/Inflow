using System;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;

namespace Inflow.Shared.Infrastructure.Postgres.Decorators;

[Decorator]
public class TransactionalCommandHandlerDecorator<T>(
    ICommandHandler<T> handler,
    UnitOfWorkTypeRegistry unitOfWorkTypeRegistry,
    IServiceProvider serviceProvider,
    ILogger<TransactionalCommandHandlerDecorator<T>> logger) : ICommandHandler<T> where T : class, ICommand
{


    public async Task HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        var unitOfWorkType = unitOfWorkTypeRegistry.Resolve<T>();
        if (unitOfWorkType is null)
        {
            await handler.HandleAsync(command, cancellationToken);
            return;
        }

        var unitOfWork = (IUnitOfWork) serviceProvider.GetRequiredService(unitOfWorkType);
        var unitOfWorkName = unitOfWorkType.Name;
        var name = command.GetType().Name.Underscore();
        logger.LogInformation("Handling: {Name} using TX ({UnitOfWorkName})...", name, unitOfWorkName);
        await unitOfWork.ExecuteAsync(() => handler.HandleAsync(command, cancellationToken));
        logger.LogInformation("Handled: {Name} using TX ({UnitOfWorkName})", name, unitOfWorkName);
    }
}

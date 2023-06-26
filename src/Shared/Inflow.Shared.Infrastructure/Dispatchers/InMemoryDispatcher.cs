using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Dispatchers;
using Inflow.Shared.Abstractions.Events;
using Inflow.Shared.Abstractions.Queries;

namespace Inflow.Shared.Infrastructure.Dispatchers;

internal sealed class InMemoryDispatcher(
    ICommandDispatcher commandDispatcher,
    IEventDispatcher eventDispatcher,
    IQueryDispatcher queryDispatcher) : IDispatcher
{
    
    public Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand => commandDispatcher.SendAsync(command, cancellationToken);

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
        => eventDispatcher.PublishAsync(@event, cancellationToken);

    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => queryDispatcher.QueryAsync(query, cancellationToken);
    
}

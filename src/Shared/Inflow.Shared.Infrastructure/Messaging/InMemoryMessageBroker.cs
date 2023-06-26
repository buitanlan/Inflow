using Inflow.Shared.Abstractions.Messaging;
using Inflow.Shared.Abstractions.Modules;
using Microsoft.Extensions.Logging;

namespace Inflow.Shared.Infrastructure.Messaging;

internal sealed class InMemoryMessageBroker(IModuleClient moduleClient, ILogger<InMemoryMessageBroker> logger) : IMessageBroker
{
    public Task PublishAsync(IMessage message, CancellationToken cancellationToken = default)
        => PublishAsync(cancellationToken, message);


    public Task PublishAsync(IMessage[] messages, CancellationToken cancellationToken = default)
        => PublishAsync(cancellationToken, messages);

    private async Task PublishAsync(CancellationToken cancellationToken, params IMessage[] messages)
    {
        if (messages is null) return;

        messages = messages.Where(x => x is not null).ToArray();
        if(!messages.Any()) return;

        var tasks = messages.Select(x => moduleClient.PublishAsync(x, cancellationToken));
        await Task.WhenAll(tasks);
    }
}

using Inflow.Shared.Abstractions.Messaging;

namespace Inflow.Shared.Infrastructure.Messaging;

internal class AsyncMessageDispatcher(IMessageChannel messageChannel) : IAsyncMessageDispatcher
{
    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class, IMessage
    {
        await messageChannel.Writer.WriteAsync(new MassageEnvelope(message), cancellationToken);
    }
}

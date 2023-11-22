namespace Inflow.Shared.Abstractions.Messaging;

public interface IMessageBroker
{
    Task PublishAsync(IMessage message, CancellationToken token = default);
    Task PublishAsync(IMessage[] messages, CancellationToken token = default);

}

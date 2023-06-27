using System.Threading.Channels;

namespace Inflow.Shared.Infrastructure.Messaging;

public class MessageChannel : IMessageChannel
{
    private readonly Channel<MassageEnvelope> _messages = Channel.CreateUnbounded<MassageEnvelope>();
    public ChannelReader<MassageEnvelope> Reader => _messages.Reader;
    public ChannelWriter<MassageEnvelope> Writer => _messages.Writer;
}

using System.Threading.Channels;

namespace Inflow.Shared.Infrastructure.Messaging;

internal interface IMessageChannel
{
    ChannelReader<MassageEnvelope> Reader { get; }
    ChannelWriter<MassageEnvelope> Writer { get; }
}

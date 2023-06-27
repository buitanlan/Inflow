using Inflow.Shared.Abstractions.Messaging;

namespace Inflow.Shared.Infrastructure.Messaging;

public record MassageEnvelope(IMessage Message);

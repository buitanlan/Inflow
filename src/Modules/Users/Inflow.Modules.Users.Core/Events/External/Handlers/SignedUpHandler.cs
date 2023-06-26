using Inflow.Shared.Abstractions.Events;

namespace Inflow.Modules.Users.Core.Events.External.Handlers;

internal sealed class SignedUpHandler : IEventHandler<SignedUp>
{
    public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }
}

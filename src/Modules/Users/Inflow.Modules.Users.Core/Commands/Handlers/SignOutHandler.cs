using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;

namespace Inflow.Modules.Users.Core.Commands.Handlers;

internal sealed class SignOutHandler(ILogger<SignOutHandler> logger) : ICommandHandler<SignOut>
{
    public async Task HandleAsync(SignOut command, CancellationToken cancellationToken = default)
    {
            
        await Task.CompletedTask;
        logger.LogInformation($"User with ID: '{command.UserId}' has signed out.");
    }
}

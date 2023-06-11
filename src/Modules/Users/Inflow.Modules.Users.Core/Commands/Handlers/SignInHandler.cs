using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Inflow.Modules.Users.Core.Entities;
// using Inflow.Modules.Users.Core.Events;
using Inflow.Modules.Users.Core.Exceptions;
using Inflow.Modules.Users.Core.Repositories;
using Inflow.Modules.Users.Core.Services;
using Inflow.Shared.Abstractions;
using Inflow.Shared.Abstractions.Auth;
using Inflow.Shared.Abstractions.Commands;
// using Inflow.Shared.Abstractions.Messaging;

namespace Inflow.Modules.Users.Core.Commands.Handlers;

internal sealed class SignInHandler(
    IUserRepository userRepository,
    IAuthManager authManager,
    IPasswordHasher<User> passwordHasher,
    IUserRequestStorage userRequestStorage,
    ILogger<SignInHandler> logger) : ICommandHandler<SignIn>
{
    public async Task HandleAsync(SignIn command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetAsync(command.Email.ToLowerInvariant())
            .NotNull(() => new InvalidCredentialsException());

        if (user.State != UserState.Active)
        {
            throw new UserNotActiveException(user.Id);
        }

        if (passwordHasher.VerifyHashedPassword(default, user.Password, command.Password) ==
            PasswordVerificationResult.Failed)
        {
            throw new InvalidCredentialsException();
        }

        var claims = new Dictionary<string, IEnumerable<string>>
        {
            ["permissions"] = user.Role.Permissions
        };

        var jwt = authManager.CreateToken(user.Id, user.Role.Name, claims: claims);
        jwt.Email = user.Email;
        // await _messageBroker.PublishAsync(new SignedIn(user.Id), cancellationToken);
        logger.LogInformation($"User with ID: '{user.Id}' has signed in.");
        userRequestStorage.SetToken(command.Id, jwt);
    }
}

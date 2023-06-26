using Inflow.Modules.Users.Core.Entities;
using Inflow.Modules.Users.Core.Events.External;
using Inflow.Modules.Users.Core.Exceptions;
using Inflow.Modules.Users.Core.Repositories;
using Inflow.Shared.Abstractions;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Time;
using Inflow.Shared.Infrastructure.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Users.Core.Commands.Handlers;

internal sealed class SignUpHandler(
    IUserRepository userRepository, 
    IRoleRepository roleRepository,
    IPasswordHasher<User> passwordHasher, 
    IClock clock,
    RegistrationOptions registrationOptions,
    IMessageBroker messageBroker,
    ILogger<SignUpHandler> logger) : ICommandHandler<SignUp>
{
    public async Task HandleAsync(SignUp command, CancellationToken cancellationToken)
    {
        if (!registrationOptions.Enabled)
        {
            throw new SignUpDisabledException();
        }
            
        var email = command.Email.ToLowerInvariant();
        var provider = email.Split("@").Last();
        if (registrationOptions.InvalidEmailProviders?.Any(x => provider.Contains(x)) is true)
        {
            throw new InvalidEmailException(email);
        }

        if (string.IsNullOrWhiteSpace(command.Password) || command.Password.Length is > 100 or < 6)
        {
            throw new InvalidPasswordException("not matching the criteria");
        }
            
        var user = await userRepository.GetAsync(email);
        if (user is not null)
        {
            throw new EmailInUseException();
        }

        var roleName = string.IsNullOrWhiteSpace(command.Role) ? Role.Default : command.Role.ToLowerInvariant();
        var role = await roleRepository.GetAsync(roleName)
            .NotNull(() => new RoleNotFoundException(roleName));

        var now = clock.CurrentDate();
        var password = passwordHasher.HashPassword(default, command.Password);
        user = new User
        {
            Id = command.UserId,
            Email = email,
            Password = password,
            Role = role,
            CreatedAt = now,
            State = UserState.Active,
        };
        await userRepository.AddAsync(user);
        await messageBroker.PublishAsync(new SignedUp(user.Id, email, role.Name), cancellationToken);
        logger.LogInformation($"User with ID: '{user.Id}' has signed up.");
    }
}

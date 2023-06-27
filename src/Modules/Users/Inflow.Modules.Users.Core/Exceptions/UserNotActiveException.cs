using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Users.Core.Exceptions;

internal class UserNotActiveException(Guid userId) : InflowException($"User with ID: '{userId}' is not active.")
{
    public Guid UserId { get; } = userId;
}
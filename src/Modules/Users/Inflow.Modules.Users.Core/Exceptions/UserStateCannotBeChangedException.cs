using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Users.Core.Exceptions;

internal class UserStateCannotBeChangedException(string state, Guid userId) : InflowException($"User state cannot be changed to: '{state}' for user with ID: '{userId}'.")
{
    public string State { get; } = state;
    public Guid UserId { get; } = userId;
}
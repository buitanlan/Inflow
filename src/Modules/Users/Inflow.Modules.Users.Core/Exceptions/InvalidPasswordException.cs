using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Users.Core.Exceptions;

internal class InvalidPasswordException(string reason) : InflowException($"Invalid password: {reason}.")
{
    public string Reason { get; } = reason;
}
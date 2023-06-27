using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Users.Core.Exceptions;

internal class InvalidEmailException(string email) : InflowException($"State is invalid: '{email}'.")
{
    public string Email { get; } = email;
}
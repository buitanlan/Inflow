using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Users.Core.Exceptions;

internal class InvalidUserStateException(string state) : InflowException($"User state is invalid: '{state}'.")
{
    public string State { get; } = state;
}
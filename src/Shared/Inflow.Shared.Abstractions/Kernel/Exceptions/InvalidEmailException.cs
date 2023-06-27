using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Shared.Abstractions.Kernel.Exceptions;

public class InvalidEmailException(string email) : InflowException($"Email: {email} is invalid.")
{
    public string Email { get; } = email;
}
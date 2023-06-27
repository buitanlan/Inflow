using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Shared.Abstractions.Kernel.Exceptions;

public class InvalidFullNameException(string fullName) : InflowException($"Full name: '{fullName}' is invalid.")
{
    public string FullName { get; } = fullName;
}
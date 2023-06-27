using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class InvalidNameException(string name) : InflowException($"Name: '{name}' is invalid.")
{
    public string Name { get; } = name;
}
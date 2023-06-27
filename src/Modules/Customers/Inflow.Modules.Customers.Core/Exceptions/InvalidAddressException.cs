using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class InvalidAddressException(string address) : InflowException($"Address: '{address}' is invalid.")
{
    public string Address { get; } = address;
}
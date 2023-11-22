using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class InvalidIdentityException(string type, string series) : InflowException($"Identity type: '{type}', series: '{series}' is invalid.")
{
    public string Type { get; } = type;
}

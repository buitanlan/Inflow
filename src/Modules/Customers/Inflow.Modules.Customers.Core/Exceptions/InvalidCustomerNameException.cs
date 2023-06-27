using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class InvalidCustomerNameException(Guid customerId) : InflowException($"Customer with ID: '{customerId}' has invalid name.")
{
    public Guid CustomerId { get; } = customerId;
}
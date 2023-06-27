using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class CustomerNotActiveException(Guid customerId) : InflowException($"Customer with ID: '{customerId}' is not active.")
{
    public Guid CustomerId { get; } = customerId;
}
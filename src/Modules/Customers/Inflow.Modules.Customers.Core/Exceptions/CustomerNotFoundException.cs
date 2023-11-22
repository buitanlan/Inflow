using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class CustomerNotFoundException(Guid customerId) : InflowException($"Customer with ID: '{customerId}' was not found.")
{
    public Guid CustomerId { get; } = customerId;
}

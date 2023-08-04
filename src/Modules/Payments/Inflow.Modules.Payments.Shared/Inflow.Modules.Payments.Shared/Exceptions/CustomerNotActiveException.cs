using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Shared.Exceptions;

public class CustomerNotActiveException(Guid customerId) : InflowException($"Customer with ID: '{customerId}' is not active.")
{
    public Guid CustomerId { get; } = customerId;
}
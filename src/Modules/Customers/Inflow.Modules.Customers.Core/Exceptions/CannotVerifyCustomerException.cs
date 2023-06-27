using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class CannotVerifyCustomerException(Guid customerId) : InflowException($"Customer with ID: '{customerId}' cannot be verified.")
{
    public Guid CustomerId { get; } = customerId;
}
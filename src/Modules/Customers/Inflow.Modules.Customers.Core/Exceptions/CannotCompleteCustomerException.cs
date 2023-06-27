using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class CannotCompleteCustomerException(Guid customerId) : InflowException($"Customer with ID: '{customerId}' cannot be completed.")
{
    public Guid CustomerId { get; } = customerId;
}
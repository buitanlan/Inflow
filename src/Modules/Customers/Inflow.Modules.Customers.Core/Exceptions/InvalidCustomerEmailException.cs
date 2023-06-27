using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class InvalidCustomerEmailException(Guid customerId) : InflowException($"Customer with ID: '{customerId}' has invalid email.")
{
    public Guid CustomerId { get; } = customerId;
}
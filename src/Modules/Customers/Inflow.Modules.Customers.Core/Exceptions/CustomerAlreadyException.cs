using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

public class CustomerAlreadyException(Guid customerId) : InflowException($"Customer with Id:'{customerId}' already existed");

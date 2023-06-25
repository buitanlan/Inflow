using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class UserNotFoundException(string message): InflowException($"User with email: '{message} was not found'");

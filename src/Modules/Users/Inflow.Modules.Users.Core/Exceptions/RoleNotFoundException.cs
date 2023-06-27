using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Users.Core.Exceptions;

internal class RoleNotFoundException(string role) : InflowException($"Role: '{role}' was not found.");
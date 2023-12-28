using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Owners.Exceptions;

public class OwnerNotFoundException(Guid ownerId) : InflowException($"Owner with ID: '{ownerId}' was not found.");

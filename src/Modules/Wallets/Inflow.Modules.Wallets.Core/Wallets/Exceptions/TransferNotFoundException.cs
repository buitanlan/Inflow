using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Wallets.Exceptions;

public class TransferNotFoundException(Guid transferId) : InflowException($"Transfer with ID: '{transferId}' was not found.");

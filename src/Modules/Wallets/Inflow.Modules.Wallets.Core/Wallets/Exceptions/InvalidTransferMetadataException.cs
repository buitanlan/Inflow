using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Wallets.Exceptions;

internal class InvalidTransferMetadataException(string metadata) : InflowException($"Transfer metadata: '{metadata}' is invalid.");

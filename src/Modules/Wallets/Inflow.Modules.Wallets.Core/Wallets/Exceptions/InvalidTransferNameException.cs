using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Wallets.Exceptions;

internal class InvalidTransferNameException(string name) : InflowException($"Transfer name: '{name}' is invalid.");

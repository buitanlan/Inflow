using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Owners.Exceptions;

internal class InvalidOwnerNameException(string name) : InflowException($"Owner name: '{name}' is invalid.");

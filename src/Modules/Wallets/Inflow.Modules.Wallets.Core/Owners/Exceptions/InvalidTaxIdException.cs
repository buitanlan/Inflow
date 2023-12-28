using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Owners.Exceptions;

public class InvalidTaxIdException(string taxId) : InflowException($"Tax ID: '{taxId}' is invalid.");

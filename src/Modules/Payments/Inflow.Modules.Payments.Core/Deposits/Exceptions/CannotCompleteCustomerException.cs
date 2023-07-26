using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Deposits.Exceptions;

internal class CannotCompleteDepositException(Guid depositId) : InflowException($"Deposit with ID: '{depositId}' cannot be completed.")
{
    public Guid DepositId { get; } = depositId;
}

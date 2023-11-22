using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Withdrawals.Exceptions;

internal class CannotCompleteWithdrawalException(Guid depositId) : InflowException($"Withdrawal with ID: '{depositId}' cannot be completed.")
{
    public Guid DepositId { get; } = depositId;
}
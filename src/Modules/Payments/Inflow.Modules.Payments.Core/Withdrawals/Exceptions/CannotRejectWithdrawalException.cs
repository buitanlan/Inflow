using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Withdrawals.Exceptions;

internal class CannotRejectWithdrawalException(Guid depositId) : InflowException($"Withdrawal with ID: '{depositId}' cannot be rejected.")
{
    public Guid DepositId { get; } = depositId;
}
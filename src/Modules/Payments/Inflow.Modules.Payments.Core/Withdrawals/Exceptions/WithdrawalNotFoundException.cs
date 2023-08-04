using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Withdrawals.Exceptions;

public class WithdrawalNotFoundException(Guid depositId) : InflowException($"Withdrawal with ID: '{depositId}' was not found.")
{
    public Guid DepositId { get; } = depositId;
}
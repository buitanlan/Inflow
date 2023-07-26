using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Deposits.Exceptions;

internal class CannotRejectDepositException(Guid depositId) : InflowException($"Deposit with ID: '{depositId}' cannot be rejected.")
{
    public Guid DepositId { get; } = depositId;
}
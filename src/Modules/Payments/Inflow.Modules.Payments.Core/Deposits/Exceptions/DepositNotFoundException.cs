using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Deposits.Exceptions;

public class DepositNotFoundException(Guid depositId) : InflowException($"Deposit with ID: '{depositId}' was not found.")
{
    public Guid DepositId { get; } = depositId;
}
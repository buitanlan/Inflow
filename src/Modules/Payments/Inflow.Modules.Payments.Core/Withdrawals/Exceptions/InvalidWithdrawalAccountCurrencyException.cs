using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Withdrawals.Exceptions;

public class InvalidWithdrawalAccountCurrencyException(Guid accountId, string currency) : InflowException($"Withdrawal account with ID: '{accountId}' has invalid currency: '{currency}'.")
{
    public Guid AccountId { get; } = accountId;
    public string Currency { get; } = currency;
}
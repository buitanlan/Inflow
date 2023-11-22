using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Deposits.Exceptions;

public class InvalidDepositAccountCurrencyException(Guid accountId, string currency) : InflowException($"Deposit account with ID: '{accountId}' has invalid currency: '{currency}'.")
{
    public Guid AccountId { get; } = accountId;
    public string Currency { get; } = currency;
}
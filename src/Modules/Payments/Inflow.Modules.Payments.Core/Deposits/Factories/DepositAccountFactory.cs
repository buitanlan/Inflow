using Inflow.Modules.Payments.Core.Deposits.Domain.Entities;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;
using Inflow.Shared.Abstractions.Time;

namespace Inflow.Modules.Payments.Core.Deposits.Factories;

internal class DepositAccountFactory(IClock clock) : IDepositAccountFactory
{
    private static readonly Random Random = new();

    public DepositAccount Create(Guid customerId, Nationality nationality, Currency currency)
    {
        var iban = $"{nationality.Value}0000{Random.Next(int.MaxValue)}0000{Random.Next(int.MaxValue)}0000";

        return new DepositAccount(Guid.NewGuid(), customerId, currency, iban, clock.CurrentDate());
    }
}

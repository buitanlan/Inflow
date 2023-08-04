using Inflow.Modules.Payments.Core.Deposits.Domain.Entities;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;

namespace Inflow.Modules.Payments.Core.Deposits.Factories;

internal interface IDepositAccountFactory
{
    DepositAccount Create(Guid customerId, Nationality nationality, Currency currency);
}
using Inflow.Shared.Abstractions.Kernel.ValueObjects;

namespace Inflow.Modules.Payments.Core.Deposits.Services;

internal interface ICurrencyResolver
{
    Currency GetForNationality(Nationality nationality);
}
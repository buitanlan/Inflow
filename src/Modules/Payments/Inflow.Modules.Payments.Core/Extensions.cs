using System.Runtime.CompilerServices;
using Inflow.Modules.Payments.Core.DAL;
using Inflow.Modules.Payments.Core.DAL.Repositories;
using Inflow.Modules.Payments.Core.Deposits.Factories;
using Inflow.Modules.Payments.Core.Deposits.Repositories;
using Inflow.Modules.Payments.Core.Deposits.Services;
using Inflow.Modules.Payments.Core.Withdrawals.Domain.Repositories;
using Inflow.Modules.Payments.Core.Withdrawals.Services;
using Inflow.Modules.Payments.Shared.Clients;
using Inflow.Modules.Payments.Shared.Repositories;
using Inflow.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Inflow.Modules.Payments.Api")]

namespace Inflow.Modules.Payments.Core;
internal static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
        => services
            .AddSingleton<ICustomerApiClient, CustomerApiClient>()
            .AddSingleton<IWithdrawalMetadataResolver, WithdrawalMetadataResolver>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IDepositRepository, DepositRepository>()
            .AddScoped<IDepositAccountRepository, DepositAccountRepository>()
            .AddScoped<IWithdrawalRepository, WithdrawalRepository>()
            .AddScoped<IWithdrawalAccountRepository, WithdrawalAccountRepository>()
            .AddSingleton<ICurrencyResolver, CurrencyResolver>()
            .AddSingleton<IDepositAccountFactory, DepositAccountFactory>()
            .AddPostgres<PaymentsDbContext>()
            .AddUnitOfWork<PaymentsUnitOfWork>();
}

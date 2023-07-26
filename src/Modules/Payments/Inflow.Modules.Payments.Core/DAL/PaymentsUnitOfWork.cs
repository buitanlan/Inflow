using Inflow.Shared.Infrastructure.Postgres;

namespace Inflow.Modules.Payments.Core.DAL;

internal class PaymentsUnitOfWork(PaymentsDbContext dbContext) : PostgresUnitOfWork<PaymentsDbContext>(dbContext);

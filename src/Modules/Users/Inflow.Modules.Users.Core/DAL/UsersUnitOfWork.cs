using Inflow.Shared.Infrastructure.Postgres;

namespace Inflow.Modules.Users.Core.DAL;

internal class UsersUnitOfWork(UsersDbContext dbContext) : PostgresUnitOfWork<UsersDbContext>(dbContext);
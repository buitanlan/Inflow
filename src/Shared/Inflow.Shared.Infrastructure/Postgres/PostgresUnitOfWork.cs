using Microsoft.EntityFrameworkCore;

namespace Inflow.Shared.Infrastructure.Postgres;

public abstract class PostgresUnitOfWork<T>(T dbContext): IUnitOfWork where T : DbContext
{
    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            await action();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

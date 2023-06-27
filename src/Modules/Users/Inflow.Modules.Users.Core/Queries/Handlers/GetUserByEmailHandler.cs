using Inflow.Modules.Users.Core.DAL;
using Inflow.Modules.Users.Core.DTO;
using Inflow.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Users.Core.Queries.Handlers;

internal sealed class GetUserByEmailHandler(UsersDbContext dbContext) : IQueryHandler<GetUserByEmail, UserDetailsDto>
{
    public async Task<UserDetailsDto> HandleAsync(GetUserByEmail query, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.Email == query.Email, cancellationToken);

        return user?.AsDetailsDto();
    }
}

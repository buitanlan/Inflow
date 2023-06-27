using Microsoft.EntityFrameworkCore;
using Inflow.Modules.Users.Core.DAL;
using Inflow.Modules.Users.Core.DTO;
using Inflow.Shared.Abstractions.Queries;

namespace Inflow.Modules.Users.Core.Queries.Handlers;

internal sealed class GetUserHandler(UsersDbContext dbContext) : IQueryHandler<GetUser, UserDetailsDto>
{
    public async Task<UserDetailsDto> HandleAsync(GetUser query, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == query.UserId, cancellationToken);

        return user?.AsDetailsDto();
    }
}

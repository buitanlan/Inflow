using System;
using Inflow.Shared.Abstractions.Auth;

namespace Inflow.Modules.Users.Core.Services;

public interface IUserRequestStorage
{
    void SetToken(Guid commandId, JsonWebToken jwt);
    JsonWebToken GetToken(Guid commandId);
}

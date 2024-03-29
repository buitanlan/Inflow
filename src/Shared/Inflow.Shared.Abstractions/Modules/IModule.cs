﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Abstractions.Modules;

public interface IModule
{
    string Name { get; }
    IEnumerable<string> Policies => default;
    void Register(IServiceCollection services);
    void Use(IApplicationBuilder app);

}

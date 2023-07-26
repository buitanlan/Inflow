using System.Reflection;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Events;
using Inflow.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inflow.Shared.Infrastructure.Modules;

public static class Extensions
{
    internal static IHostBuilder ConfigureModules(this IHostBuilder builder)
        => builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            foreach (var settings in GetStrings("*"))
            {
                cfg.AddJsonFile(settings);
            }
            IEnumerable<string> GetStrings(string pattern)
                => Directory.EnumerateFiles(ctx.HostingEnvironment.ContentRootPath,
                    $"modules.{pattern}.json", SearchOption.AllDirectories);
        });


    internal static IServiceCollection AddModuleRequests(this IServiceCollection services, IList<Assembly> assemblies)
    {
        services
            .AddModuleRegistry(assemblies)
            .AddSingleton<IModuleSubscriber, ModuleSubscriber>()
            .AddSingleton<IModuleClient, ModuleClient>()
            .AddSingleton<IModuleSerializer, JsonModuleSerializer>();

        return services;
    }

    public static IServiceCollection AddModuleRegistry(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        var registry = new ModuleRegistry();
        var types = assemblies.SelectMany(x => x.GetTypes()).ToArray();
        var commandTypes = types.Where(x => x.IsClass && typeof(ICommand).IsAssignableFrom(x)).ToArray();
        var eventTypes = types.Where(x => x.IsClass && typeof(IEvent).IsAssignableFrom(x)).ToArray();

        services.AddSingleton<IModuleRegistry>(sp =>
        {
            var commandDispatcher = sp.GetRequiredService<ICommandDispatcher>();
            var eventDispatcher = sp.GetRequiredService<IEventDispatcher>();

            foreach (var type in commandTypes)
            {
                var registration = new ModuleBroadcastRegistration(type, (@event, cancellationToken) =>
                    (Task)commandDispatcher.GetType().GetMethod(nameof(commandDispatcher.SendAsync))
                        ?.MakeGenericMethod(type)
                        .Invoke(commandDispatcher, new[] { @event, cancellationToken }));
                registry.AddBroadcastAction(registration);
            }

            foreach (var type in eventTypes)
            {
                var registration = new ModuleBroadcastRegistration(type, (@event, cancellationToken) =>
                    (Task)eventDispatcher.GetType().GetMethod(nameof(eventDispatcher.PublishAsync))
                        ?.MakeGenericMethod(type)
                        .Invoke(eventDispatcher, new[] { @event, cancellationToken }));
                registry.AddBroadcastAction(registration);
            }

            return registry;
        });

        return services;

    }

    public static IModuleSubscriber UseModuleRequests(this IApplicationBuilder app)
        => app.ApplicationServices.GetRequiredService<IModuleSubscriber>();

}

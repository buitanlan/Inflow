using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Inflow.Shared.Infrastructure.Modules;

internal static class Extensions
{
    public static IHostBuilder ConfigureModules(this IHostBuilder builder)
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
    

}
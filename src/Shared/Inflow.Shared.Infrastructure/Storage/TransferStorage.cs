using Inflow.Shared.Abstractions.Storage;
using Microsoft.Extensions.Caching.Memory;

namespace Inflow.Shared.Infrastructure.Storage;

internal class TransferStorage(IMemoryCache cache) : ITransferStorage
{
    public void Set<T>(string key, T value, TimeSpan? duration = null)
        => cache.Set(key, value, duration ?? TimeSpan.FromSeconds(5));

    public T Get<T>(string key) => cache.Get<T>(key);
}

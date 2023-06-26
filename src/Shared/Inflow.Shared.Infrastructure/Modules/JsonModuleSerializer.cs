using System.Text.Json;
using Inflow.Shared.Abstractions.Modules;

namespace Inflow.Shared.Infrastructure.Modules;

internal sealed class JsonModuleSerializer : IModuleSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };

    public byte[] Serialize<T>(T value) => JsonSerializer.SerializeToUtf8Bytes(value, SerializerOptions);

    public T Deserialize<T>(byte[] value) =>  JsonSerializer.Deserialize<T>(value, SerializerOptions);

    public object Deserialize(byte[] value, Type type) => JsonSerializer.Deserialize(value,type, SerializerOptions);
}

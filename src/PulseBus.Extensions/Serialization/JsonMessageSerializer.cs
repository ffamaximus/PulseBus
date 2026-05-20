using System.Text.Json;
using PulseBus.Abstractions;

namespace PulseBus.Extensions.Serialization;

public class JsonMessageSerializer : IMessageSerializer
{
    public byte[] Serialize<T>(T message)
        => JsonSerializer.SerializeToUtf8Bytes(message);

    public T Deserialize<T>(byte[] data)
        => JsonSerializer.Deserialize<T>(data);
}
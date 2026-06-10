using System.Text.Json;
using System.Text.Json.Serialization;
using PulseBus.Outbox.Abstractions;

namespace PulseBus.Outbox.Extensions;

public class JsonOutboxSerializer : IOutboxSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public string Serialize(object evt)
    {
        return JsonSerializer.Serialize(evt, Options);
    }

    public object Deserialize(string payload, string type)
    {
        var targetType = Type.GetType(type, throwOnError: true)!;
        return JsonSerializer.Deserialize(payload, targetType, Options)!;
    }
}

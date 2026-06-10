namespace PulseBus.Outbox.Abstractions;

public interface IOutboxSerializer
{
    string Serialize(object evt);
    object Deserialize(string payload, string type);
}
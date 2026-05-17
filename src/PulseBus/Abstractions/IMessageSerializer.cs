namespace PulseBus.Core.Abstractions;

public interface IMessageSerializer
{
    byte[] Serialize<T>(T message);
    T Deserialize<T>(byte[] data);
}
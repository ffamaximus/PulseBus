namespace PulseBus.Core.Models;

public class MessageEnvelope
{
    public string MessageId { get; set; }
    public string CorrelationId { get; set; }
    public string Topic { get; set; }
    public byte[] Payload { get; set; }
    public MessageHeaders Headers { get; set; } = new();
}

using PulseBus.Core.Models;

namespace PulseBus.Core.Abstractions;

public interface IBusProvider
{
    IMessageProducer CreateProducer();
    IMessageConsumer CreateConsumer(string topic, Func<MessageEnvelope, IMessageContext, Task> handler);
}
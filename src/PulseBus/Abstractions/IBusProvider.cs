using System;
using System.Threading.Tasks;
using PulseBus.Models;

namespace PulseBus.Abstractions;

public interface IBusProvider
{
    IMessageProducer CreateProducer();
    IMessageConsumer CreateConsumer(string topic, Func<MessageEnvelope, IMessageContext, Task> handler);
}
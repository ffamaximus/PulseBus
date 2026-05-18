using System.Threading.Tasks;
using PulseBus.Abstractions;
using PulseBus.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PulseBus.RabbitMQ.Consumer;

public class RabbitMqMessageContext : IMessageContext
{
    private readonly IChannel _channel;
    private readonly BasicDeliverEventArgs _args;

    public RabbitMqMessageContext(IChannel channel, BasicDeliverEventArgs args)
    {
        _channel = channel;
        _args = args;
    }

    public string MessageId => _args.BasicProperties.MessageId;
    public string CorrelationId => _args.BasicProperties.CorrelationId;
    public MessageHeaders Headers => new();

    public Task AcknowledgeAsync()
    {
        _channel.BasicAckAsync(_args.DeliveryTag, multiple: false);
        return Task.CompletedTask;
    }

    public Task RejectAsync(bool requeue = false)
    {
        _channel.BasicNackAsync(_args.DeliveryTag, multiple: false, requeue);
        return Task.CompletedTask;
    }
}
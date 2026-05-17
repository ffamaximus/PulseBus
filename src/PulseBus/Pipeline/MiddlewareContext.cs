using PulseBus.Core.Abstractions;
using PulseBus.Core.Models;

namespace PulseBus.Core.Pipeline;

public class MiddlewareContext
{
    public MessageEnvelope Envelope { get; set; }
    public IMessageContext MessageContext { get; set; }
    public IServiceProvider Services { get; set; }
}
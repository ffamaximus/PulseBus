using System;
using PulseBus.Abstractions;
using PulseBus.Models;

namespace PulseBus.Pipeline;

public class MiddlewareContext
{
    public MessageEnvelope Envelope { get; set; }
    public IMessageContext MessageContext { get; set; }
    public IServiceProvider Services { get; set; }
}
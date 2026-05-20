using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PulseBus.Abstractions;
using PulseBus.Pipeline;

namespace PulseBus.Extensions.Middlewares;

public class LoggingMiddleware : IMessageMiddleware
{
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(MiddlewareContext context, MiddlewareDelegate next)
    {
        _logger.LogInformation("Processing message {MessageId} on topic {Topic}",
            context.Envelope.MessageId, context.Envelope.Topic);

        await next(context);

        _logger.LogInformation("Message {MessageId} processed successfully",
            context.Envelope.MessageId);
    }
}

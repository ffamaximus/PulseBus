using PulseBus.Core.Pipeline;

namespace PulseBus.Core.Abstractions;

public interface IMessageMiddleware
{
    Task InvokeAsync(MiddlewareContext context, MiddlewareDelegate next);
}
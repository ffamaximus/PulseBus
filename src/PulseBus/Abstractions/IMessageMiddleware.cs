using System.Threading.Tasks;
using PulseBus.Pipeline;

namespace PulseBus.Abstractions;

public interface IMessageMiddleware
{
    Task InvokeAsync(MiddlewareContext context, MiddlewareDelegate next);
}
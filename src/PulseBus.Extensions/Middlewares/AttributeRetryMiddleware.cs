using System;
using System.Threading.Tasks;
using PulseBus.Abstractions;
using PulseBus.Pipeline;

namespace PulseBus.Extensions.Middlewares;

public class AttributeRetryMiddleware : IMessageMiddleware
{
    private readonly int _attempts;
    private readonly int _delaySeconds;

    public AttributeRetryMiddleware(int attempts, int delaySeconds)
    {
        _attempts = attempts;
        _delaySeconds = delaySeconds;
    }

    public async Task InvokeAsync(MiddlewareContext context, MiddlewareDelegate next)
    {
        int attempt = 0;

        while (true)
        {
            try
            {
                attempt++;
                await next(context);
                return; // success
            }
            catch (Exception ex)
            {
                if (attempt >= _attempts)
                {
                    Console.WriteLine($"The message failed after {_attempts} attempts: {ex.Message}");
                    throw; // Failures - DLQ
                }

                Console.WriteLine($"Error, retry in {_delaySeconds}s (Attempt {attempt}/{_attempts})");
                await Task.Delay(_delaySeconds * 1000);
            }
        }
    }
}

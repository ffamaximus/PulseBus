using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PulseBus.Abstractions;
using PulseBus.Models;
using PulseBus.Pipeline;

namespace PulseBus.Extensions.Middlewares;

public class RetryMiddleware : IMessageMiddleware
{
    public async Task InvokeAsync(MiddlewareContext context, MiddlewareDelegate next)
    {
        var retryPolicy = context.Services.GetRequiredService<IMessageRetryPolicy>();
        var attempt = 0;

        while (true)
        {
            try
            {
                await next(context);
                return;
            }
            catch (Exception ex)
            {
                var retryContext = new RetryContext
                {
                    Attempt = attempt,
                    Exception = ex
                };

                if (!retryPolicy.ShouldRetry(retryContext))
                    throw;

                await Task.Delay(retryPolicy.GetDelay(retryContext));
                attempt++;
            }
        }
    }
}
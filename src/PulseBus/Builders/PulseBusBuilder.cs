using Microsoft.Extensions.DependencyInjection;
using PulseBus.Models;

namespace PulseBus.Builders;

public class PulseBusBuilder(IServiceCollection services)
{
    public IServiceCollection Services { get; } = services;
    public BusOptions Options { get; } = new();
}
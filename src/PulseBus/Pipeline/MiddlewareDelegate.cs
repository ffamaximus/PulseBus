using System.Threading.Tasks;

namespace PulseBus.Pipeline;

public delegate Task MiddlewareDelegate(MiddlewareContext context);
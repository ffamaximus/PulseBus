namespace PulseBus.Core.Models;

public class RetryContext
{
    public int Attempt { get; set; }
    public Exception Exception { get; set; }
}
using System;

namespace PulseBus.Exceptions;

public class ProviderException : Exception
{
    public string ProviderName { get; }

    public ProviderException(string message, string providerName, Exception innerException = null)
        : base(message, innerException)
    {
        ProviderName = providerName;
    }

}
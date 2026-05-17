using System;

namespace PulseBus.Exceptions;

public class ProviderException : Exception
{
    public ProviderException(string message) : base(message)
    {
    
    }

}
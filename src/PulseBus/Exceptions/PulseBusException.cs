using System;

namespace PulseBus.Exceptions;

public class PulseBusException : Exception
{
    public PulseBusException()
    {
    }

    public PulseBusException(string message) : base(message)
    {
    }

    public PulseBusException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
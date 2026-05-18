using System;

namespace PulseBus.Exceptions;

public class SerializationException : Exception
{
    public string SerializerType { get; }

    public SerializationException(string message, string serializerType, Exception innerException = null)
        : base(message, innerException)
    {
        SerializerType = serializerType;
    }
}
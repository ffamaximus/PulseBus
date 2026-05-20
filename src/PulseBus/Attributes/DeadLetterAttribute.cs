using System;

namespace PulseBus.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DeadLetterAttribute : Attribute
{
    public string QueueName { get; }
    public DeadLetterAttribute(string queueName)
    {
        QueueName = queueName;
    }
}

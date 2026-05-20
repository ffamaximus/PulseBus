using System;

namespace PulseBus.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RetryAttribute(int attempts, int delaySeconds) : Attribute
{
    public int Attempts { get; } = attempts;
    public int DelaySeconds { get; } = delaySeconds;
}

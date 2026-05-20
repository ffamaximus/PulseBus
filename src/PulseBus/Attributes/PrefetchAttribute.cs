using System;

namespace PulseBus.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PrefetchAttribute(ushort count) : Attribute
{
    public ushort Count { get; } = count;
}

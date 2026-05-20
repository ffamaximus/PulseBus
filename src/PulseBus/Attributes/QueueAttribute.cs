using System;

namespace PulseBus.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class QueueAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
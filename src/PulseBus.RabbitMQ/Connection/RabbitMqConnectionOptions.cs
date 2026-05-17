namespace PulseBus.RabbitMQ.Connection;

public class RabbitMqConnectionOptions
{
    public required string Host { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}
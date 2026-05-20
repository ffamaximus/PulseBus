namespace PulseBus.RabbitMQ.Connection;

public class RabbitMqConnectionOptions
{
    public string Host { get; set; } = "";
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool UseTls { get; set; } = false;
}
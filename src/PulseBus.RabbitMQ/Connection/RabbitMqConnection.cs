using RabbitMQ.Client;

namespace PulseBus.RabbitMQ.Connection;

public class RabbitMqConnection
{
    private readonly RabbitMqConnectionOptions _options;
    private IConnection _connection;
    private readonly Lock _lock = new();

    public RabbitMqConnection(RabbitMqConnectionOptions options)
    {
        _options = options;
    }

    public async Task<IChannel> CreateChannelAsync()
    {
        EnsureConnection();
        return await _connection.CreateChannelAsync();
    }

    private void EnsureConnection()
    {
        if (_connection.IsOpen)
            return;

        lock (_lock)
        {
            if (_connection.IsOpen)
                return;

            var factory = new ConnectionFactory
            {
                HostName = _options.Host,
                UserName = _options.Username,
                Password = _options.Password
            };

            _connection = factory.CreateConnectionAsync().Result;
        }
    }
}
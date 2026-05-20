#nullable enable
using System.Net.Security;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace PulseBus.RabbitMQ.Connection;

public class RabbitMqConnection
{
    private readonly RabbitMqConnectionOptions _options;
    private IConnection? _connection;

    public RabbitMqConnection(RabbitMqConnectionOptions options)
    {
        _options = options;
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection is { IsOpen: true })
            return _connection;

        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.Username,
            Password = _options.Password
            
        };

        if (_options.UseTls)
        {
            factory.Ssl.Enabled = true;
            factory.Ssl.ServerName = _options.Host;
            factory.Ssl.AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch |
                                                 SslPolicyErrors.RemoteCertificateChainErrors;
        }

        _connection = await factory.CreateConnectionAsync();
        return _connection;
    }
}
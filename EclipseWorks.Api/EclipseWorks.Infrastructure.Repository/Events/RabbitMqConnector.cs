using RabbitMQ.Client;

namespace EclipseWorks.Infrastructure.Events
{
    public class RabbitMqConnector
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection = null;
        private IChannel? _channel = null;
        public RabbitMqConnector(ConnectionFactory factory)
        {
            _factory = factory;
        }

        protected async Task<IConnection> CreateConnection()
        {
            if (_connection == null)
                _connection = await _factory.CreateConnectionAsync();

            return _connection;
        }

        public async Task<IChannel> CreateChannel()
        {
            if (_channel == null)
            {
                var connection = await CreateConnection();
                _channel = await connection.CreateChannelAsync();
            }
            return _channel;
        }
    }

}

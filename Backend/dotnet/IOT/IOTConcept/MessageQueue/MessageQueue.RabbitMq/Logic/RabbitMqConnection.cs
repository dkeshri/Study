using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading.Channels;

namespace MessageQueue.RabbitMq.Logic
{
    public class RabbitMqConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        private readonly string directExchangeName = "weather_direct";
        public RabbitMqConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connectionFactory = factory;
        }
        public IConnection CreateConnection()
        {
            var connection = _connectionFactory.CreateConnection();
            createDirectExchange(connection);
            return connection;
        }
        
        private void createDirectExchange(IConnection connection)
        {
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(directExchangeName, ExchangeType.Direct);
            if (channel.IsOpen)
            {
                channel.Close();
                channel.Dispose();
            }
        }
    }
}

using MessageQueue.RabbitMq.Extensions;
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
        private readonly string directExchangeName = "weather_direct";
        public RabbitMqConnection(IConfiguration configuration)
        {
            string HostName = configuration.GetRabbitMqHostName();
            string UserName = configuration.GetRabbitMqUserName();
            string Password = configuration.GetRabbitMqPassword();
            int Port = configuration.GetRabbitMqPort();
            var factory = new ConnectionFactory()
            {
                HostName = HostName,
                Port = Port,
                UserName = UserName,
                Password = Password
            };
            factory.ClientProvidedName = "Sender/Receiver";
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

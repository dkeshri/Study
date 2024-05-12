using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Runtime.InteropServices;
using System.Threading.Channels;

namespace MessageQueue.RabbitMq.Logic
{
    public class RabbitMqConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        public RabbitMqConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connectionFactory = factory;
        }
        public IConnection CreateConnection()
        {
            return _connectionFactory.CreateConnection();
        }

    }
}

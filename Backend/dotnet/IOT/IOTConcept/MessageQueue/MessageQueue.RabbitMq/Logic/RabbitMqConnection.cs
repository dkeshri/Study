using MessageQueue.RabbitMq.Extensions;
using MessageQueue.RabbitMq.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading.Channels;

namespace MessageQueue.RabbitMq.Logic
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConnection _connection;
        private IModel _channel;
        private string _exchangeName;
        public IModel Channel { get => CreateChannelIfClosed(_connection); }
        public string ExchangeName { get => _exchangeName; }
        public RabbitMqConnection(IConfiguration configuration)
        {
            string hostName = configuration.GetRabbitMqHostName();
            string userName = configuration.GetRabbitMqUserName();
            string password = configuration.GetRabbitMqPassword();
            _exchangeName = configuration.GetRabbitMqExchangeName();
            int port = configuration.GetRabbitMqPort();
            string clientProvidedName = configuration.GetRabbitMqClientProvidedName();

            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };

            factory.ClientProvidedName = clientProvidedName;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            createDirectExchange(_channel, _exchangeName);
        }
        
        private void createDirectExchange(IModel channel,string exchangeName)
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        }

        private IModel CreateChannelIfClosed(IConnection connection)
        {
            if(_channel.IsOpen)
                return _channel;
            _channel = connection.CreateModel();
            return _channel;
        }
    }
}

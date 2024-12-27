using MessageQueue.RabbitMq.Extensions;
using MessageQueue.RabbitMq.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;


namespace MessageQueue.RabbitMq.Logic
{
    internal sealed class RabbitMqConnection : IRabbitMqConnection, IDisposable
    {
        private readonly IConnection _connection;
        private IModel _channel;
        private string _exchangeName;
        private bool _isDisposing = false;
        private string _queueName;
        public IModel Channel { get => CreateChannelIfClosed(_connection); }
        public string ExchangeName { get => _exchangeName; }
        public string QueueName { get => _queueName; }
        public RabbitMqConnection(IConfiguration configuration)
        {
            string hostName = configuration.GetRabbitMqHostName();
            string userName = configuration.GetRabbitMqUserName();
            string password = configuration.GetRabbitMqPassword();
            _exchangeName = configuration.GetRabbitMqExchangeName();
            _queueName = configuration.GetRabbitMqQueueName() ?? "defaultQueue";
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (_isDisposing)
            {
                return;
            }
            if (disposing)
            {
                if(_channel.IsOpen)
                {
                    _channel.Close();
                    _channel.Dispose();
                }
                if (_connection.IsOpen)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
            _isDisposing = true;
        }
    }
}

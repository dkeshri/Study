using MessageQueue.RabbitMq.Extensions;
using MessageQueue.RabbitMq.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Net.Sockets;


namespace MessageQueue.RabbitMq.Logic
{
    internal sealed class RabbitMqConnection : IRabbitMqConnection, IDisposable
    {
        private readonly IConnection? _connection;
        private IModel? _channel;
        private string _exchangeName;
        private bool _isDisposing = false;
        private string _queueName;
        public IModel? Channel { get => CreateChannelIfClosed(_connection); }
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
            try
            {
                if (!IsRabbitMqReachable(hostName, port))
                {
                    Console.WriteLine($"RabbitMQ host: {hostName}:{port} is not reachable. Connection will not be created.");
                    return;
                }
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
            catch (Exception ex) 
            { 
                throw new Exception("Error ouccr while creating connection",ex);
            }
            
        }
        
        private void createDirectExchange(IModel channel,string exchangeName)
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        }

        private IModel? CreateChannelIfClosed(IConnection? connection)
        {
            if(connection == null)
            {
                return null;
            }

            if(_channel!= null && _channel.IsOpen)
                return _channel;
            _channel = connection.CreateModel();
            return _channel;
        }

        private bool IsRabbitMqReachable(string hostName, int port)
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    var result = tcpClient.BeginConnect(hostName, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                    if (!success) return false;

                    tcpClient.EndConnect(result);
                    return true;
                }
            }
            catch
            {
                return false;
            }
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
                if(_channel !=null && _channel.IsOpen)
                {
                    _channel.Close();
                    _channel.Dispose();
                }
                if (_connection != null && _connection.IsOpen)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
            _isDisposing = true;
        }
    }
}

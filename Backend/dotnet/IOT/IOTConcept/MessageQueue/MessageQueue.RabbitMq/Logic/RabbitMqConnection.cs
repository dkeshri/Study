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
        private IConnectionFactory _connectionFactory;
        private readonly string hostName;
        private readonly int port;

        private IConnection? Connection { get => CreateConnection(_connectionFactory); }
        public IModel? Channel { get => CreateChannelIfClosed(Connection); }
        public string ExchangeName { get => _exchangeName; }
        public string QueueName { get => _queueName; }
        public RabbitMqConnection(IConfiguration configuration)
        {

            hostName = configuration.GetRabbitMqHostName();
            port = configuration.GetRabbitMqPort();
            _exchangeName = configuration.GetRabbitMqExchangeName();
            _queueName = configuration.GetRabbitMqQueueName() ?? "defaultQueue";

            string userName = configuration.GetRabbitMqUserName();
            string password = configuration.GetRabbitMqPassword();
            string clientProvidedName = configuration.GetRabbitMqClientProvidedName();
            try
            {
                _connectionFactory = new ConnectionFactory()
                {
                    HostName = hostName,
                    Port = port,
                    UserName = userName,
                    Password = password,
                    ClientProvidedName = clientProvidedName
                };

                if (!IsRabbitMqReachable(hostName, port)) return;
                _connection = _connectionFactory.CreateConnection();
                _connection.ConnectionShutdown += OnConnectionShutdown;
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

        private IConnection? CreateConnection(IConnectionFactory connectionFactory)
        {
            if (!IsRabbitMqReachable(hostName, port)) return null;
            try
            {
                return connectionFactory.CreateConnection();
            }
            catch (Exception) { 
                return null;
            }
            
        }

        private bool IsRabbitMqReachable(string hostName, int port)
        {
            string errorMessage = $"RabbitMQ host: {hostName}:{port} is not reachable. Connection can not be created!";
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    var result = tcpClient.BeginConnect(hostName, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                    if (!success)
                    {
                        Console.WriteLine(errorMessage);
                        return false;
                    }

                    tcpClient.EndConnect(result);
                    return true;
                }
            }
            catch
            {
                Console.WriteLine(errorMessage);
                return false;
            }
        }

        private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection is shutdown! closing the open channel");
            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
                _channel.Dispose();
                Console.WriteLine("Channel is closed!");
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

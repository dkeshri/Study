﻿using Dkeshri.MessageQueue.RabbitMq.Extensions;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Net.Sockets;


namespace MessageQueue.RabbitMq.Logic
{
    internal sealed class RabbitMqConnection : IRabbitMqConnection, IDisposable
    {
        private IConnection? _connection;
        private IModel? _channel;
        private bool _isDisposing = false;
        private IConnectionFactory _connectionFactory;
        private readonly string hostName;
        private readonly int port;
        private bool _isConfirmSelected = false;
        private string userName;
        private string password;
        private string clientProvidedName;
        private QueueConfig? _queueConfig;
        private ExchangeConfig? _exchangeConfig;
        private bool _registerSenderServices;
        private bool _registerReceiverServices;
        private IConnection? Connection { get => CreateConnection(_connectionFactory); }
        public IModel? Channel { get => CreateChannelIfClosed(Connection); }
        public QueueConfig? Queue { get => _queueConfig; }
        public ExchangeConfig? Exchange { get => _exchangeConfig; }
        public bool RegisterSenderServices { get => _registerSenderServices; }
        public bool RegisterReceiverServices { get => _registerReceiverServices; }

        public RabbitMqConnection(RabbitMqConfig rabbitMqConfig)
        {
            _queueConfig = rabbitMqConfig.Queue;
            _exchangeConfig = rabbitMqConfig.Exchange;
            hostName = rabbitMqConfig.HostName;
            port = rabbitMqConfig.Port;
            userName = rabbitMqConfig.UserName;
            password = rabbitMqConfig.Password;
            clientProvidedName = rabbitMqConfig.ClientProvidedName;
            _registerReceiverServices = rabbitMqConfig.RegisterReceiverServices;
            _registerSenderServices = rabbitMqConfig.RegisterSenderServices;
            _connectionFactory = CreateConnectionFactory();
        }

        private ConnectionFactory CreateConnectionFactory()
        {
            try
            {
                return new ConnectionFactory()
                {
                    HostName = hostName,
                    Port = port,
                    UserName = userName,
                    Password = password,
                    ClientProvidedName = clientProvidedName
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error ouccr while creating connection", ex);
            }
        }

        private IModel? CreateChannelIfClosed(IConnection? connection)
        {
            if (connection == null)
            {
                return null;
            }

            if (_channel != null && _channel.IsOpen)
                return _channel;
            _channel = connection.CreateModel();
            return _channel;
        }

        private IConnection? CreateConnection(IConnectionFactory connectionFactory)
        {
            if (!IsRabbitMqReachable(hostName, port)) return null;
            try
            {
                if (_connection != null && _connection.IsOpen)
                    return _connection;
                _connection = connectionFactory.CreateConnection();
                _connection.ConnectionShutdown += OnConnectionShutdown;
                return _connection;
            }
            catch (Exception)
            {
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
            _isConfirmSelected = false;
            Console.WriteLine("Connection is shutdown! closing the open channel if any!");
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
                if (_channel != null && _channel.IsOpen)
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
        public void EnableConfirmIfNotSelected()
        {
            if (_channel == null || _channel.IsClosed) return;
            if (_isConfirmSelected == false)
            {
                _channel.ConfirmSelect();
            }
            else
            {
                _isConfirmSelected = true;
            }

        }

    }
}

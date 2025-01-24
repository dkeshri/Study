using Dkeshri.MessageQueue.Extensions;
using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Handlers;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Logic;
using MessageQueue.RabbitMq.Logic;
using MessageQueue.RabbitMq.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Dkeshri.MessageQueue.RabbitMq.Extensions
{
    public static class ServiceCollectionExtensions
    {      
        public static RabbitMqConfig AddRabbitMqServices(this MessageBroker messageBroker, Action<RabbitMqConfig> configuration)
        {
            RabbitMqConfig rabbitMqConfig = new RabbitMqConfig();
            configuration.Invoke(rabbitMqConfig);
            rabbitMqConfig.RegisterSenderServices = messageBroker.RegisterSenderServices;
            rabbitMqConfig.RegisterReceiverServices = messageBroker.RegisterReceiverServices;
            rabbitMqConfig.ClientProvidedName = messageBroker.ClientProvidedName;
            return messageBroker.AddRabbitMqServices(rabbitMqConfig);
        }

        public static RabbitMqConfig AddRabbitMqServices(this MessageBroker messageBroker, RabbitMqConfig rabbitMqConfig)
        {
            if(messageBroker.Services == null)
            {
                throw new InvalidOperationException("Please provide IServiceCollection reference, Can not be null!");
            }

            messageBroker.Services.AddSingleton<IRabbitMqConnection>(sp =>
            {
                return new RabbitMqConnection(rabbitMqConfig);
            });

            messageBroker.Services.AddSingleton<MessageBrokerFactory, RabbitMqMessageBroker>();
            rabbitMqConfig.AddRequeriedServices(messageBroker);
            return rabbitMqConfig;
        }

        public static void UseQueue(this RabbitMqConfig rabbitMqConfig, Action<QueueConfig> action)
        {
            QueueConfig queueConfig = new QueueConfig();
            rabbitMqConfig.Queue = queueConfig;
            action.Invoke(queueConfig);
        }

        public static void UseExchange(this RabbitMqConfig rabbitMqConfig, Action<ExchangeConfig> action)
        {
            ExchangeConfig exchangeConfig = new ExchangeConfig();
            rabbitMqConfig.Exchange = exchangeConfig;
            action.Invoke(exchangeConfig);
        }
        private static void AddRequeriedServices(this RabbitMqConfig config, MessageBroker messageBroker)
        {

            if (config.RegisterReceiverServices) 
            {
                messageBroker.Services.AddMessageReceiverQueueService();
            }
        }

        private static void AddMessageReceiverQueueService(this IServiceCollection services) 
        {
            services.AddSingleton<IMessageHandler, MessageReceiverHandler>();
            services.AddHostedService<MessageReceiverQueueService>();
        }

    }
}

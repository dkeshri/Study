using Dkeshri.MessageQueue.Extensions;
using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Handlers;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Logic;
using MessageQueue.RabbitMq.Logic;
using MessageQueue.RabbitMq.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Dkeshri.MessageQueue.RabbitMq.Extensions
{
    public static class ServiceCollectionExtensions
    {      
        public static void AddRabbitMqServices(this MessageBroker messageBroker, Action<RabbitMqConfig> configuration)
        {
            RabbitMqConfig rabbitMqConfig = new RabbitMqConfig();
            rabbitMqConfig.Queue = new QueueConfig();
            rabbitMqConfig.Queue.Arguments = new Dictionary<string, object>();
            
            rabbitMqConfig.Exchange = new ExchangeConfig();
            rabbitMqConfig.Exchange.Arguments = new Dictionary<string, object>();
            
            configuration.Invoke(rabbitMqConfig);
            
            rabbitMqConfig.RegisterSenderServices = messageBroker.RegisterSenderServices;
            rabbitMqConfig.RegisterReceiverServices = messageBroker.RegisterReceiverServices;
            rabbitMqConfig.ClientProvidedName = messageBroker.ClientProvidedName;
            messageBroker.AddRabbitMqServices(rabbitMqConfig);
        }

        public static void AddRabbitMqServices(this MessageBroker messageBroker, RabbitMqConfig rabbitMqConfig)
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
        }

        private static void AddRequeriedServices(this RabbitMqConfig config, MessageBroker messageBroker)
        {

            if (config.RegisterReceiverServices) 
            {
                messageBroker.Services.AddMessageReceiverQueueService();
            }
        }

        public static void AddMessageReceiverQueueService(this IServiceCollection services) 
        {
            services.AddSingleton<IMessageHandler, MessageReceiverHandler>();
            services.AddHostedService<MessageReceiverQueueService>();
        }

    }
}

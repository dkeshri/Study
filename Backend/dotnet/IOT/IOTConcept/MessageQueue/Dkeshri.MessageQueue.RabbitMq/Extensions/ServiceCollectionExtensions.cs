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
            //rabbitMqConfig.AddRequestedServices(messageBroker);
        }

        private static void AddRequestedServices(this RabbitMqConfig config, MessageBroker messageBroker)
        {
            if (config.RegisterSenderServices)
            {
                messageBroker.Services.AddSenderService();
            }

            if (config.RegisterReceiverServices) 
            {
                messageBroker.Services.AddRbbitMqMessageReceiverServiceForQueue();
            }
        }
        private static void AddSenderService(this IServiceCollection services)
        {
            services.AddSingleton<ISendMessage, SendMessage>();
        }

        public static void AddRbbitMqMessageReceiverServiceForQueue(this IServiceCollection services) 
        {
            services.AddSingleton<IMessageReceiver,MessageReceiverHandler>();
            services.AddHostedService<MessageReceiverFromQueueService>();
        }
        public static void AddRbbitMqMessageReceiverServiceForExchange(this IServiceCollection services)
        {
            services.AddHostedService<ReceiveMessageFromExchangeService>();
        }
    }
}

using Dkeshri.MessageQueue.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Dkeshri.MessageQueue.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessageBroker(this IServiceCollection services, Action<MessageBroker> action)
        {
            MessageBroker messageBroker = new MessageBroker(services);
            action.Invoke(messageBroker);
            services.AddMessageBroker(messageBroker);
        }
        public static void AddMessageBroker(this IServiceCollection services, MessageBroker messageBroker)
        {
            messageBroker.Services.AddSingleton<IStartup>(sp =>
            {
                var factory = sp.GetRequiredService<MessageBrokerFactory>();
                IStartup startup = factory.CreateInitializer();
                return startup;
            });

            if (messageBroker.RegisterSenderServices)
            {
                messageBroker.Services.AddSingleton<IMessageSender>(sp =>
                {
                    var factory = sp.GetRequiredService<MessageBrokerFactory>();
                    IMessageSender sender = factory.CreateSender();
                    return sender;
                });
            }

            if (messageBroker.RegisterReceiverServices) {
                messageBroker.Services.AddSingleton<IMessageReceiver>(sp =>
                {
                    var factory = sp.GetRequiredService<MessageBrokerFactory>();
                    IMessageReceiver receiver = factory.CreateReceiver();
                    return receiver;
                });
            
            }
            
        }

    }
}

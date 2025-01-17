using Dkeshri.MessageQueue.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (messageBroker.RegisterSenderServices)
            {
                messageBroker.Services.AddSingleton<ISendMessage>(sp =>
                {
                    var factory = sp.GetRequiredService<MessageBrokerFactory>();
                    ISendMessage sender = factory.CreateSender();
                    return sender;
                });
            }
            
        }

    }
}

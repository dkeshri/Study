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
            MessageBroker messageBroker = new MessageBroker();
            messageBroker.Services = services;
            action.Invoke(messageBroker);
        }
        

    }
}

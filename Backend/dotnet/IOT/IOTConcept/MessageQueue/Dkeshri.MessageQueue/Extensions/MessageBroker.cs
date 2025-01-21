using Microsoft.Extensions.DependencyInjection;

namespace Dkeshri.MessageQueue.Extensions
{
    public class MessageBroker
    {
        private IServiceCollection _services;
        public IServiceCollection Services { get => _services; }
        public bool RegisterSenderServices { get; set; } = false;
        public bool RegisterReceiverServices { get; set; } = false;
        public string ClientProvidedName { get; set; } = "Unknown";

        public MessageBroker(IServiceCollection services)
        {
            _services = services;
        }
    }
}

using Dkeshri.DataSync.DBChangeEmitter.Interfaces;
using Dkeshri.MessageQueue.Interfaces;

namespace Dkeshri.DataSync.DBChangeEmitter.Services
{
    internal class MessageBrokerInitService : IMessageBrokerInitService
    {
        IStartup _startup;
        public MessageBrokerInitService(IStartup startup)
        {
            _startup = startup;
        }

        public void InitMessageBroker()
        {
            _startup.OnStart();
        }
    }
}

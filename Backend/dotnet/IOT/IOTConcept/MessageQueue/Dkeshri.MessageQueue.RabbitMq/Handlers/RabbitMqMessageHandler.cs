using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using Dkeshri.MessageQueue.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Dkeshri.MessageQueue.RabbitMq.Handlers
{
    internal sealed class RabbitMqMessageHandler : IMessageHandler
    {
        private readonly IMessageReceiver _messageReceiver;

        public RabbitMqMessageHandler(IMessageReceiver messageReceiver)
        {
            _messageReceiver = messageReceiver;
        }

        void IMessageHandler.HandleMessage(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var messageHandler = _messageReceiver.MessageHandler;
            if (messageHandler == null)
            {
                Console.WriteLine("Please configure IMessageReceiver!, MessageHandler is null");
                channel.BasicReject(ea.DeliveryTag, requeue: false);
                return;
            }

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var isMessageProcessed = messageHandler.Invoke(message);

            if (isMessageProcessed)
            {
                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            else
            {
                channel.BasicReject(ea.DeliveryTag, requeue: false);
            }
        }
    }
}

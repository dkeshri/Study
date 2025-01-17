using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;

namespace Dkeshri.MessageQueue.RabbitMq.Handlers
{
    internal class MessageReceiverHandler : IMessageHandler, IMessageReceiver
    {
        public Predicate<string>? MessageHandler { get; set; }

        void IMessageHandler.HandleMessage(object? model, BasicDeliverEventArgs ea, IModel channel)
        {

            if (MessageHandler == null)
            {
                Console.WriteLine("Please configure IMessageReceiver!, MessageHandler is null");
                return;
            }
            bool isMessageProcessed = false;
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            isMessageProcessed = MessageHandler.Invoke(message);

            if (isMessageProcessed == true)
            {
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }

    }
}

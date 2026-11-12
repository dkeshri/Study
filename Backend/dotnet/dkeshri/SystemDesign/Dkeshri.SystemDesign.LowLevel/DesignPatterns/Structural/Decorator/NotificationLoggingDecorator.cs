using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.DesignPatterns.Structural.Decorator
{
    internal class NotificationLoggingDecorator : INotification
    {
        private readonly INotification _notification;
        public NotificationLoggingDecorator(INotification notification)
        {
            _notification = notification;
        }
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"Logging: Sending notification to {recipient}");
            _notification.Send(recipient, message);
            Console.WriteLine($"Logging: Notification sent to {recipient}");
        }
    }
}

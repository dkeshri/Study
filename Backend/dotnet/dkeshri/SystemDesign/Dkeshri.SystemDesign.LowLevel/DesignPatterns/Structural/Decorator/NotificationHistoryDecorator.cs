using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.DesignPatterns.Structural.Decorator
{
    internal class NotificationHistoryDecorator : INotification
    {
        private readonly INotification _notification;
        private readonly List<string> _history;
        public NotificationHistoryDecorator(INotification notification)
        {
            _notification = notification;
            _history = new List<string>();
        }
        public void Send(string recipient, string message)
        {
            _notification.Send(recipient, message);
            _history.Add($"Sent to {recipient}: {message}");
            Console.WriteLine($"History: {recipient}: {message}");
        }
        public IEnumerable<string> GetHistory()
        {
            return _history.AsReadOnly();
        }              
    }
}

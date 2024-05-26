using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.MediatR.Handlers
{
    internal class NotificationHandler : INotificationHandler<NotificationMessage>
    {
        public Task Handle(NotificationMessage notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"MediatR: Message from ${nameof(NotificationMessage)} ${notification.Message}");
            return Task.CompletedTask;
        }
    }
}

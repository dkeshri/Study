using Dkeshri.SystemDesign.LowLevel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.DesignPatterns.Structural.Decorator
{
    public class DecoratorClientApplication : IExecute
    {
        public void run()
        {

            // initially we were sending email notification.
            //INotification emailNotification = new EmailNotification();
            //emailNotification.Send("john@example.com", "Hello, this is a test email notification.");

            // we will use decorator pattern to add logging and history functionality to the existing email notification functionality without modifying the existing code.
            // Now we want to log the notification as well as save history. So we will use the decorator to add logging functionality.
            INotification emailNotification = new EmailNotification();
            emailNotification = new NotificationLoggingDecorator(emailNotification);
            emailNotification = new NotificationHistoryDecorator(emailNotification);
            emailNotification.Send("john@example.com", "Hello, this is a test email notification.");
        }

    }
}

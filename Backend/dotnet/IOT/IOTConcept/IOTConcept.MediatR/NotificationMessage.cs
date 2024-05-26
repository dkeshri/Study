using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.MediatR
{
    public class NotificationMessage : INotification
    {
        public string? Message { get; set; } 
    }
}

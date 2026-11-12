using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.DesignPatterns.Structural.Decorator
{
    public interface INotification
    {
        void Send(string recipient, string message);
    }
}

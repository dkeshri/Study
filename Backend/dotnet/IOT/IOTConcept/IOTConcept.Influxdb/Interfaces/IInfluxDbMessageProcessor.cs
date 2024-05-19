using IOTConcept.Influxdb.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.Influxdb.Interfaces
{
    public interface IInfluxDbMessageProcessor
    {
        public void WriteMessage(Message message);
        public Task ReadMessage();
    }
}

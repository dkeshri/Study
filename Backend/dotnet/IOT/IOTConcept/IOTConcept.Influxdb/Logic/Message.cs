using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.Influxdb.Logic
{
    public class Message
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public double Value { get; set; }
    }
}

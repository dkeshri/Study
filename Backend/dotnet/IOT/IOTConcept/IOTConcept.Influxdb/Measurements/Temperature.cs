using InfluxDB.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.Influxdb.Measurements
{
    [Measurement("temperature")]
    public class Temperature
    {
        [Column("country", IsTag = true)] 
        public string? Country { get; set; }

        [Column("city", IsTag = true)]
        public string? City { get; set; }

        [Column("value")] 
        public double Value { get; set; }

        [Column(IsTimestamp = true)] 
        public DateTime Time { get; set; } = DateTime.UtcNow;
    }
}

using InfluxDB.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfluxDb
{
    [Measurement("temperature")]
    public class Temperature
    {
        [Column("location", IsTag = true)] public string? Location { get; set; }

        [Column("value")] public double Value { get; set; }

        [Column(IsTimestamp = true)] public DateTime Time { get; set; }
    }
}

using InfluxDB.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.Influxdb.Interfaces
{
    public interface IInfluxDbClientFactory
    {
        public IInfluxDBClient InfluxDBClient { get; }
        public string Bucket { get; }
        public string Org { get; }
    }
}

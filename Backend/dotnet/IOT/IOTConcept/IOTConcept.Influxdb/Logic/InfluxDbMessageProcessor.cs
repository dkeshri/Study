using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using IOTConcept.Influxdb.Interfaces;
using IOTConcept.Influxdb.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.Influxdb.Logic
{
    public class InfluxDbMessageProcessor : IInfluxDbMessageProcessor
    {
        private readonly IInfluxDBClient _influxDBClient;
        private readonly string _bucket;
        private readonly string _org;
        public InfluxDbMessageProcessor(IInfluxDbClientFactory influxDbClientFactory)
        {
            _influxDBClient = influxDbClientFactory.InfluxDBClient;
            _bucket = influxDbClientFactory.Bucket;
            _org = influxDbClientFactory.Org;
        }

        public void WriteMessage(Message message)
        {
            var temperature = new Temperature { Location = message.Tag, Value = message.Value, Time = DateTime.UtcNow };
            _influxDBClient.GetWriteApi()
                .WriteMeasurement<Temperature>(temperature, WritePrecision.Ns, _bucket, _org);
        }

        public async Task ReadMessage()
        {
            var flux = $"from(bucket:\"{_bucket}\") |> range(start: 0) |> sort(columns: [\"_time\"], desc: false)";

            var fluxTables = await _influxDBClient.GetQueryApi().QueryAsync(flux, _org);
            fluxTables.ForEach(fluxTable =>
            {
                var fluxRecords = fluxTable.Records;
                fluxRecords.OrderBy(c => c.GetTime()).ToList();
                fluxRecords.ForEach(fluxRecord =>
                {
                    Console.WriteLine($"{fluxRecord.GetTime()}: {fluxRecord.GetValue()}");
                });
            });
        }
    }
}

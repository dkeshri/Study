using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfluxDb
{
    public class InfluxMessageProcesser
    {
        private readonly string host = InfluxDbConfiguration.INFLUX_DB_HOST_URL;
        private readonly string username = InfluxDbConfiguration.USERNAME;
        private readonly string password = InfluxDbConfiguration.USER_PASSWORD;
        private readonly string org = InfluxDbConfiguration.ORG;
        private readonly string token = InfluxDbConfiguration.TOKEN;
        private readonly string bucket = InfluxDbConfiguration.BUCKET_NAME;

        private readonly InfluxDBClient influxDBClient;
        public InfluxMessageProcesser()
        {
            this.influxDBClient = GetAuthenticatedClient();
        }
        public void WriteMessage(Message message)
        {
            var temperature = new Temperature { Location = message.Tag, Value = message.Value, Time = DateTime.UtcNow };
            this.influxDBClient.GetWriteApi()
                .WriteMeasurement<Temperature>(temperature, WritePrecision.Ns, bucket, org);
        }

        public async Task ReadMessage()
        {
            var flux = $"from(bucket:\"{bucket}\") |> range(start: 0)";

            var fluxTables = await influxDBClient.GetQueryApi().QueryAsync(flux, org);
            fluxTables.ForEach(fluxTable =>
            {
                var fluxRecords = fluxTable.Records;
                fluxRecords.ForEach(fluxRecord =>
                {
                    Console.WriteLine($"{fluxRecord.GetTime()}: {fluxRecord.GetValue()}");
                });
            });
        }
        private InfluxDBClient GetAuthenticatedClient()
        {
            var option = new InfluxDBClientOptions.Builder()
                .Url(host)
                .AuthenticateToken(token)
                .Org(org)
                .Bucket(bucket)
                .Build();

            var client = new InfluxDBClient(option);
            return client;
        }
    }
}

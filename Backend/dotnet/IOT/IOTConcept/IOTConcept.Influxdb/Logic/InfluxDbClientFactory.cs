using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using IOTConcept.Influxdb.Interfaces;
using IOTConcept.Influxdb.Measurements;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.Influxdb.Logic
{
    public sealed class InfluxDbClientFactory : IInfluxDbClientFactory
    {
        private readonly string host = InfluxDbConfiguration.INFLUX_DB_HOST_URL;
        private readonly string username = InfluxDbConfiguration.USERNAME;
        private readonly string password = InfluxDbConfiguration.USER_PASSWORD;
        private readonly string org = InfluxDbConfiguration.ORG;
        private readonly string token = InfluxDbConfiguration.TOKEN;
        private readonly string bucket = InfluxDbConfiguration.BUCKET_NAME;

        private readonly IInfluxDBClient _influxDBClient;
        public IInfluxDBClient InfluxDBClient { get => _influxDBClient; }
        public string Bucket { get => bucket; }
        public string Org { get => org; }
        public InfluxDbClientFactory(IConfiguration configuration)
        {
            _influxDBClient = GetAuthenticatedClient();
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

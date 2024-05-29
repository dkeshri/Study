using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using IOTConcept.Influxdb.Interfaces;
using IOTConcept.Influxdb.Measurements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.Influxdb.Logic
{
    public sealed class InfluxDbClientFactory : IInfluxDbClientFactory
    {
        private InfluxDbConfigurationOptions _influxDbConfig;
        private readonly IInfluxDBClient _influxDBClient;
        public IInfluxDBClient InfluxDBClient { get => _influxDBClient; }
        public string Bucket { get => _influxDbConfig.BucketName; }
        public string Org { get => _influxDbConfig.Org; }
        public InfluxDbClientFactory(IOptions<InfluxDbConfigurationOptions> options)
        {
            _influxDbConfig = options.Value;
            _influxDBClient = GetAuthenticatedClient();
        }
        
        private InfluxDBClient GetAuthenticatedClient()
        {
            var option = new InfluxDBClientOptions.Builder()
                .Url(_influxDbConfig.InfluxUrl)
                .AuthenticateToken(_influxDbConfig.Token)
                .Org(_influxDbConfig.Org)
                .Bucket(_influxDbConfig.BucketName)
            .Build();

            var client = new InfluxDBClient(option);
            return client;
        }
    }
}

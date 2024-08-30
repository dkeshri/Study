using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using IOTConcept.Influxdb.Interfaces;
using Microsoft.Extensions.Options;


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
            _influxDBClient = GetAuthenticatedClientAsync().GetAwaiter().GetResult();
        }

        private async Task<InfluxDBClient> GetAuthenticatedClientAsync()
        {
            using var client = new InfluxDBClient(_influxDbConfig.InfluxUrl, _influxDbConfig.UserName, _influxDbConfig.Password);

            var organization = await GetOrganizationAsync(client);
            var bucket = await GetOrCreateBucketAsync(client, organization);
            var token = await GetInfluxApiTokenAsync(client, organization, bucket);

            var option = new InfluxDBClientOptions.Builder()
                .Url(_influxDbConfig.InfluxUrl)
                .AuthenticateToken(token)
                .Org(_influxDbConfig.Org)
                .Bucket(_influxDbConfig.BucketName)
                .Build();

            var finalClient = new InfluxDBClient(option);

            return finalClient;
        }

        private async Task<string> GetInfluxApiTokenAsync(InfluxDBClient client, Organization organization, Bucket bucket)
        {
            var permissionResource = new PermissionResource
            { Type = PermissionResource.TypeBuckets, OrgID = organization.Id, Id = bucket.Id };
            var rBucket = new Permission(Permission.ActionEnum.Read, permissionResource);
            var wBucket = new Permission(Permission.ActionEnum.Write, permissionResource);
            List<Permission> permissions = new List<Permission>() { rBucket, wBucket };
            var authorzation = await client.GetAuthorizationsApi()
                .CreateAuthorizationAsync(organization, permissions);
            return authorzation.Token;
        }

        private async Task<Organization> GetOrganizationAsync(InfluxDBClient client)
        {
            var organizations = await client.GetOrganizationsApi()
                .FindOrganizationsAsync();
            return organizations.FirstOrDefault(x => x.Name == _influxDbConfig.Org) ?? new Organization();
        }

        private async Task<Bucket> GetOrCreateBucketAsync(InfluxDBClient client, Organization organization)
        {
            var retaintionPeriod = TimeSpan.FromDays(_influxDbConfig.RetentionPeriodDays);

            var bucketRetentionRule = new BucketRetentionRules(BucketRetentionRules.TypeEnum.Expire, (int)retaintionPeriod.TotalSeconds);

            var bucket = await client.GetBucketsApi()
                .FindBucketByNameAsync(_influxDbConfig.BucketName) ?? await client.GetBucketsApi()
                .CreateBucketAsync(_influxDbConfig.BucketName, bucketRetentionRule, organization.Id);

            return bucket;

        }
    }
}

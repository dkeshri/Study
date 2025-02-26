namespace ApiGateway.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetTokenSecret(this IConfiguration configuration)
        {
            return configuration.GetSection("Jwt:Token:Secret").Value!;
        }
    }
}
